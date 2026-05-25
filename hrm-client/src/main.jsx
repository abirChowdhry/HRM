import React, { useEffect, useMemo, useState } from "react";
import { createRoot } from "react-dom/client";
import {
  BadgeDollarSign,
  Banknote,
  ArrowRightLeft,
  BriefcaseBusiness,
  Building2,
  CalendarClock,
  Check,
  ChevronRight,
  CircleDollarSign,
  ClipboardList,
  Edit3,
  Gift,
  KeyRound,
  LayoutDashboard,
  Loader2,
  LogIn,
  LogOut,
  Plus,
  Search,
  ShieldCheck,
  Trash2,
  UserRound,
  UsersRound,
  WalletCards,
  X
} from "lucide-react";
import hrmLogo from "./assets/hrm-logo-transparent.png";
import "./styles.css";

const API_BASE = import.meta.env.VITE_API_BASE_URL || "/api";
const AUTH_KEY = "hrm-auth";

function getStoredAuth() {
  try {
    return JSON.parse(localStorage.getItem(AUTH_KEY) || "null");
  } catch {
    return null;
  }
}

const emptyData = {
  businessUnits: [],
  departments: [],
  designations: [],
  employmentTypes: [],
  employees: [],
  payrollPolicies: [],
  payrollElements: [],
  salaryDetails: [],
  bonuses: [],
  transferHistory: []
};

const emptyEmployee = {
  intEmployeeBasicInfoId: 0,
  strEmployeeCode: "",
  strEmployeeName: "",
  intBusinessUnitId: "",
  intDepartmentId: "",
  intDesignationId: "",
  intEmploymentTypeId: "",
  strGender: "",
  strMaritalStatus: "",
  strBloodGroup: "",
  dteDateOfBirth: "",
  dteJoiningDate: "",
  isSalaryHold: false,
  intCreatedBy: 1,
  intUpdatedBy: 1
};

const emptyPolicy = {
  intPayrollPolicyId: 0,
  intBusinessUnitId: "",
  strPayrollPolicyName: "",
  isSalaryDivideByActualMonthDays: true,
  intGrossSalaryDevidedByDays: 30,
  intGrossSalaryRoundDigits: 2,
  isGrossSalaryRoundUp: false,
  isGrossSalaryRoundDown: false,
  intNetPayableSalaryRoundDigits: 2,
  isNetPayableSalaryRoundUp: false,
  isNetPayableSalaryRoundDown: false,
  intCreatedBy: 1,
  intUpdatedBy: 1
};

const emptyElement = {
  intPayrollElementId: 0,
  strPayrollElementName: "",
  intBusinessUnitId: "",
  isBasicElement: false,
  isSalaryElement: true,
  intCreatedBy: 1,
  intUpdatedBy: 1
};

const emptyBonus = {
  intBonusSetypId: 0,
  strBonusSetupName: "",
  intBusinessUnitId: "",
  intDepartmentId: "",
  intEmployementTypeId: "",
  intServiceLengthMonths: "",
  numPercentage: "",
  intCreatedBy: 1,
  intUpdatedBy: 1
};

const emptyTransfer = {
  intEmpTransferNPromotionId: 0,
  intEmployeeId: "",
  intBusinessUnitId: "",
  intOldDepartmentId: "",
  intNewDepartmenId: "",
  intOldDesignationId: "",
  intNewDesignationId: "",
  isTransfer: false,
  isPromotion: false,
  intCreatedBy: 1,
  intUpdatedBy: 1
};

const navItems = [
  { id: "dashboard", label: "Dashboard", icon: LayoutDashboard },
  { id: "employees", label: "Employees", icon: UsersRound },
  { id: "movement", label: "Transfer", icon: ArrowRightLeft },
  { id: "setup", label: "Setup", icon: Building2 },
  { id: "payroll", label: "Payroll", icon: WalletCards },
  { id: "salary", label: "Salary", icon: Banknote },
  { id: "bonus", label: "Bonus", icon: Gift }
];

function titleCase(value) {
  return String(value || "")
    .replace(/([A-Z])/g, " $1")
    .replace(/^./, (char) => char.toUpperCase())
    .trim();
}

function toNumber(value) {
  if (value === "" || value === null || value === undefined) return 0;
  return Number(value);
}

function toNullableNumber(value) {
  if (value === "" || value === null || value === undefined) return null;
  return Number(value);
}

function isBlank(value) {
  return value === "" || value === null || value === undefined;
}

function collectRequiredErrors(fields) {
  return fields.filter((field) => isBlank(field.value)).map((field) => field.label);
}

async function api(path, options = {}) {
  const token = getStoredAuth()?.token;
  const response = await fetch(`${API_BASE}${path}`, {
    headers: {
      "Content-Type": "application/json",
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
      ...(options.headers || {})
    },
    ...options
  });

  if (!response.ok) {
    let message = "Request failed";
    try {
      const data = await response.json();
      message = data.message || data.Message || message;
    } catch {
      message = response.statusText || message;
    }
    throw new Error(message);
  }

  const text = await response.text();
  return text ? JSON.parse(text) : null;
}

function useHrmData() {
  const [data, setData] = useState(emptyData);
  const [isDemoMode, setDemoMode] = useState(true);
  const [loading, setLoading] = useState(false);
  const [notice, setNotice] = useState("Sign in to load workspace");

  async function loadData(auth = getStoredAuth()) {
    if (!auth?.token) {
      setData(emptyData);
      setDemoMode(true);
      setLoading(false);
      setNotice("Sign in to load workspace");
      return;
    }

    setLoading(true);
    try {
      const [businessUnits, departments, designations, employmentTypes] = await Promise.all([
        api("/Basic/GetBusinessUnits"),
        api("/Basic/GetDepartments"),
        api("/Basic/GetDesignations"),
        api("/Basic/GetEmployementTypes")
      ]);

      const businessUnitIds = businessUnits.map((unit) => unit.intBusinessUnitId);
      const selectedBusinessUnitId = businessUnitIds[0] || 0;
      const employeeResponses = await Promise.all(
        businessUnitIds.map((businessUnitId) =>
          api("/Employee/EmployeeLanding", {
            method: "POST",
            body: JSON.stringify({
              intBusinessunitId: businessUnitId,
              isHeaderNeed: true,
              pageNo: 1,
              pageSize: 1000,
              isPaginated: false,
              searchTxt: ""
            })
          })
        )
      );

      const employees = employeeResponses.flatMap((response) => response?.data || response?.Data || []);

      const [payrollPolicies, payrollElements, salaryDetails, bonuses] = selectedBusinessUnitId
        ? await Promise.all([
            api(`/Payroll/PayrollPolicyLanding?businessUnitId=${selectedBusinessUnitId}`, { method: "POST" }),
            api(`/Payroll/PayrollElementLanding?businessUnitId=${selectedBusinessUnitId}`, { method: "POST" }),
            api(`/Salary/SalaryDetailsLanding?businessUnitId=${selectedBusinessUnitId}`, { method: "POST" }),
            api(`/Bonus/BonusLanding?intBusinessUnitId=${selectedBusinessUnitId}`, { method: "POST" })
          ])
        : [[], [], [], []];

      const transferResponses = await Promise.all(
        businessUnitIds.map((businessUnitId) =>
          api(
            `/Employee/EmployeeTransferNPromotionHistory?dteFromDate=2000-01-01&dteToDate=2100-12-31&intBusinessUnitId=${businessUnitId}`,
            { method: "POST" }
          )
        )
      );
      const transferHistory = transferResponses.flatMap((response) => response || []);

      setData({
        businessUnits,
        departments,
        designations,
        employmentTypes,
        employees,
        payrollPolicies,
        payrollElements,
        salaryDetails,
        bonuses,
        transferHistory
      });
      setDemoMode(false);
      setNotice("Connected to backend API");
    } catch {
      setData(emptyData);
      setDemoMode(true);
      setNotice("No backend connection: showing empty workspace");
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadData();
  }, []);

  function localInsert(collection, item, idKey) {
    setData((current) => {
      const nextId = Math.max(0, ...current[collection].map((row) => Number(row[idKey] || 0))) + 1;
      return {
        ...current,
        [collection]: [{ ...item, [idKey]: item[idKey] || nextId }, ...current[collection]]
      };
    });
  }

  function localUpsert(collection, item, idKey) {
    setData((current) => {
      const exists = current[collection].some((row) => Number(row[idKey]) === Number(item[idKey]));
      if (!exists) {
        const nextId = Math.max(0, ...current[collection].map((row) => Number(row[idKey] || 0))) + 1;
        return { ...current, [collection]: [{ ...item, [idKey]: item[idKey] || nextId }, ...current[collection]] };
      }
      return {
        ...current,
        [collection]: current[collection].map((row) => (Number(row[idKey]) === Number(item[idKey]) ? item : row))
      };
    });
  }

  function localDelete(collection, idKey, id) {
    setData((current) => ({
      ...current,
      [collection]: current[collection].filter((row) => Number(row[idKey]) !== Number(id))
    }));
  }

  return {
    data,
    isDemoMode,
    loading,
    notice,
    reload: loadData,
    setNotice,
    localInsert,
    localUpsert,
    localDelete
  };
}

function App() {
  const [auth, setAuth] = useState(getStoredAuth);
  const store = useHrmData();
  const [active, setActive] = useState("dashboard");
  const [searchText, setSearchText] = useState("");
  const [employeeForm, setEmployeeForm] = useState(emptyEmployee);
  const [policyForm, setPolicyForm] = useState(emptyPolicy);
  const [elementForm, setElementForm] = useState(emptyElement);
  const [bonusForm, setBonusForm] = useState(emptyBonus);
  const [transferForm, setTransferForm] = useState(emptyTransfer);
  const [setupDraft, setSetupDraft] = useState({ businessUnit: "", department: "", designation: "", employmentType: "" });
  const [setupEdit, setSetupEdit] = useState({ type: "", id: "" });
  const [formErrors, setFormErrors] = useState({
    employee: [],
    policy: [],
    element: [],
    bonus: [],
    transfer: []
  });

  const {
    businessUnits,
    departments,
    designations,
    employmentTypes,
    employees,
    payrollPolicies,
    payrollElements,
    salaryDetails,
    bonuses,
    transferHistory
  } = store.data;

  const selectedBusinessUnit = businessUnits[0] || null;
  const selectedBusinessUnitId = selectedBusinessUnit?.intBusinessUnitId || 0;
  const businessUnitSummary = businessUnits.length === 0
    ? "No business units"
    : `${businessUnits.length} unit${businessUnits.length === 1 ? "" : "s"}`;

  async function authenticate(mode, values) {
    const response = await api(`/Auth/${mode === "signup" ? "Signup" : "Login"}`, {
      method: "POST",
      body: JSON.stringify(values)
    });
    localStorage.setItem(AUTH_KEY, JSON.stringify(response));
    setAuth(response);
    store.setNotice(`Welcome, ${response.fullName || response.email}`);
    await store.reload();
  }

  function logout() {
    localStorage.removeItem(AUTH_KEY);
    setAuth(null);
    setActive("dashboard");
    store.setNotice("Signed out");
  }

  const filteredEmployees = useMemo(() => {
    const term = searchText.trim().toLowerCase();
    const rows = term
      ? employees.filter((employee) =>
          [
            employee.strEmployeeCode,
            employee.strEmployeeName,
            employee.strDepartmentName,
            employee.strDesignationName,
            employee.strEmploymentTypeName
          ]
            .join(" ")
            .toLowerCase()
            .includes(term)
        )
      : employees;

    return [...rows].sort((left, right) => {
      const leftId = Number(left.intEmployeeBasicInfoId || left.intEmployeeId || 0);
      const rightId = Number(right.intEmployeeBasicInfoId || right.intEmployeeId || 0);
      return leftId - rightId;
    });
  }, [employees, searchText]);

  const totalSalary = salaryDetails.reduce((sum, row) => sum + Number(row.numGrossSalary || 0), 0);
  const activeCount = employees.filter((employee) => employee.isActive !== false).length;

  async function saveEmployee(event) {
    event.preventDefault();
    const errors = collectRequiredErrors([
      { label: "Code", value: employeeForm.strEmployeeCode?.trim() },
      { label: "Full name", value: employeeForm.strEmployeeName?.trim() },
      { label: "Business unit", value: employeeForm.intBusinessUnitId },
      { label: "Department", value: employeeForm.intDepartmentId },
      { label: "Designation", value: employeeForm.intDesignationId },
      { label: "Employment type", value: employeeForm.intEmploymentTypeId },
      { label: "Joining date", value: employeeForm.dteJoiningDate }
    ]);

    if (errors.length > 0) {
      setFormErrors((current) => ({ ...current, employee: errors }));
      return;
    }

    setFormErrors((current) => ({ ...current, employee: [] }));
    const department = departments.find((item) => Number(item.intDepartmentId) === Number(employeeForm.intDepartmentId));
    const designation = designations.find((item) => Number(item.intDesignationId) === Number(employeeForm.intDesignationId));
    const business = businessUnits.find((item) => Number(item.intBusinessUnitId) === Number(employeeForm.intBusinessUnitId));
    const employment = employmentTypes.find((item) => Number(item.intEmployementId) === Number(employeeForm.intEmploymentTypeId));

    const payload = {
      ...employeeForm,
      intBusinessUnitId: toNumber(employeeForm.intBusinessUnitId),
      intDepartmentId: toNumber(employeeForm.intDepartmentId),
      intDesignationId: toNumber(employeeForm.intDesignationId),
      intEmploymentTypeId: toNumber(employeeForm.intEmploymentTypeId),
      dteDateOfBirth: employeeForm.dteDateOfBirth || null,
      dteJoiningDate: employeeForm.dteJoiningDate
    };

    try {
      if (!store.isDemoMode) {
        await api("/Employee/CreateNUpdateEmployee", { method: "POST", body: JSON.stringify(payload) });
        await store.reload();
      } else {
        store.localUpsert(
          "employees",
          {
            ...payload,
            strEmployeeCode: payload.strEmployeeCode,
            strEmployeeName: payload.strEmployeeName,
            strDepartmentName: department?.strDepartmentName || "",
            strDesignationName: designation?.strDesignationName || "",
            strBusinessUnitName: business?.strBusinessUnitName || "",
            intEmployementTyperId: payload.intEmploymentTypeId,
            strEmploymentTypeName: employment?.strEmployementName || "",
            dteDateOfBirth: payload.dteDateOfBirth,
            isActive: true
          },
          "intEmployeeBasicInfoId"
        );
      }
      setEmployeeForm(emptyEmployee);
      store.setNotice("Employee saved");
    } catch (error) {
      store.setNotice(error.message);
    }
  }

  async function removeEmployee(employee) {
    const id = employee.intEmployeeBasicInfoId || employee.intEmployeeId;
    try {
      if (!store.isDemoMode && id) {
        await api(`/Employee/DeleteEmployee?employeeId=${id}`, { method: "POST" });
        await store.reload();
      } else {
        store.localDelete("employees", "strEmployeeCode", employee.strEmployeeCode);
      }
      store.setNotice("Employee removed");
    } catch (error) {
      store.setNotice(error.message);
    }
  }

  async function saveSetup(type) {
    const value = setupDraft[type].trim();
    if (!value) {
      store.setNotice(`${titleCase(type)} name is required`);
      return;
    }

    const endpointMap = {
      businessUnit: `/Basic/CreateBusinessUnit?businessUnitName=${encodeURIComponent(value)}`,
      department: `/Basic/CreateDeparment?depatmentName=${encodeURIComponent(value)}`,
      designation: `/Basic/CreateDesignation?designationName=${encodeURIComponent(value)}`,
      employmentType: `/Basic/CreateEmployementType?employementTypeName=${encodeURIComponent(value)}`
    };
    const updateEndpointMap = {
      businessUnit: `/Basic/UpdateBusinessUnit?businessUnitId=${setupEdit.id}&businessUnitName=${encodeURIComponent(value)}`,
      department: `/Basic/UpdateDeparment?departmentId=${setupEdit.id}&departmentName=${encodeURIComponent(value)}`,
      designation: `/Basic/UpdateDesignation?designationId=${setupEdit.id}&designationName=${encodeURIComponent(value)}`,
      employmentType: `/Basic/UpdateEmployementType?employementTypeId=${setupEdit.id}&employementTypeName=${encodeURIComponent(value)}`
    };
    const localMap = {
      businessUnit: ["businessUnits", "intBusinessUnitId", "strBusinessUnitName"],
      department: ["departments", "intDepartmentId", "strDepartmentName"],
      designation: ["designations", "intDesignationId", "strDesignationName"],
      employmentType: ["employmentTypes", "intEmployementId", "strEmployementName"]
    };
    const isEditing = setupEdit.type === type && setupEdit.id;
    const [collection, idKey, labelKey] = localMap[type];

    try {
      if (!store.isDemoMode) {
        await api(isEditing ? updateEndpointMap[type] : endpointMap[type], { method: "POST" });
        await store.reload();
      } else {
        if (isEditing) {
          store.localUpsert(collection, { [idKey]: setupEdit.id, [labelKey]: value }, idKey);
        } else {
          store.localInsert(collection, { [labelKey]: value }, idKey);
        }
      }
      setSetupDraft((current) => ({ ...current, [type]: "" }));
      setSetupEdit({ type: "", id: "" });
      store.setNotice(`${titleCase(type)} ${isEditing ? "updated" : "created"}`);
    } catch (error) {
      store.setNotice(error.message);
    }
  }

  function editSetup(type, item, idKey, labelKey) {
    setSetupEdit({ type, id: item[idKey] });
    setSetupDraft((current) => ({ ...current, [type]: item[labelKey] || "" }));
  }

  function clearSetupEdit(type) {
    setSetupDraft((current) => ({ ...current, [type]: "" }));
    if (setupEdit.type === type) {
      setSetupEdit({ type: "", id: "" });
    }
  }

  async function deleteSetup(type, id) {
    const endpointMap = {
      businessUnit: `/Basic/DeleteBusinessUnit?businessUnitId=${id}`,
      department: `/Basic/DeleteDeparment?departmentId=${id}`,
      designation: `/Basic/DeleteDesignation?designationId=${id}`,
      employmentType: `/Basic/DeleteEmployementType?employementTypeId=${id}`
    };
    const localMap = {
      businessUnit: ["businessUnits", "intBusinessUnitId"],
      department: ["departments", "intDepartmentId"],
      designation: ["designations", "intDesignationId"],
      employmentType: ["employmentTypes", "intEmployementId"]
    };
    const [collection, idKey] = localMap[type];

    try {
      if (!store.isDemoMode) {
        await api(endpointMap[type], { method: "POST" });
        await store.reload();
      } else {
        store.localDelete(collection, idKey, id);
      }
      if (setupEdit.type === type && Number(setupEdit.id) === Number(id)) {
        clearSetupEdit(type);
      }
      store.setNotice(`${titleCase(type)} deleted`);
    } catch (error) {
      store.setNotice(error.message);
    }
  }

  async function savePolicy(event) {
    event.preventDefault();
    const errors = collectRequiredErrors([
      { label: "Policy name", value: policyForm.strPayrollPolicyName?.trim() },
      { label: "Business unit", value: policyForm.intBusinessUnitId },
      { label: "Salary days", value: policyForm.intGrossSalaryDevidedByDays }
    ]);

    if (errors.length > 0) {
      setFormErrors((current) => ({ ...current, policy: errors }));
      return;
    }

    setFormErrors((current) => ({ ...current, policy: [] }));
    const business = businessUnits.find((item) => Number(item.intBusinessUnitId) === Number(policyForm.intBusinessUnitId));
    const payload = {
      ...policyForm,
      intPayrollPolicyId: toNumber(policyForm.intPayrollPolicyId),
      intBusinessUnitId: toNumber(policyForm.intBusinessUnitId),
      intGrossSalaryDevidedByDays: toNumber(policyForm.intGrossSalaryDevidedByDays),
      intGrossSalaryRoundDigits: toNumber(policyForm.intGrossSalaryRoundDigits),
      intNetPayableSalaryRoundDigits: toNumber(policyForm.intNetPayableSalaryRoundDigits)
    };
    try {
      if (!store.isDemoMode) {
        await api("/Payroll/CreateNUpdatePayrollPolicy", { method: "POST", body: JSON.stringify(payload) });
        await store.reload();
      } else {
        store.localUpsert("payrollPolicies", { ...payload, strBusinessUnitName: business?.strBusinessUnitName || "" }, "intPayrollPolicyId");
      }
      setPolicyForm(emptyPolicy);
      store.setNotice("Payroll policy saved");
    } catch (error) {
      store.setNotice(error.message);
    }
  }

  async function removePolicy(id) {
    try {
      if (!store.isDemoMode) {
        await api(`/Payroll/DeletePayrollPolicy?policyId=${id}`, { method: "POST" });
        await store.reload();
      } else {
        store.localDelete("payrollPolicies", "intPayrollPolicyId", id);
      }
      store.setNotice("Payroll policy removed");
    } catch (error) {
      store.setNotice(error.message);
    }
  }

  async function saveElement(event) {
    event.preventDefault();
    const errors = collectRequiredErrors([
      { label: "Element name", value: elementForm.strPayrollElementName?.trim() },
      { label: "Business unit", value: elementForm.intBusinessUnitId }
    ]);

    if (errors.length > 0) {
      setFormErrors((current) => ({ ...current, element: errors }));
      return;
    }

    setFormErrors((current) => ({ ...current, element: [] }));
    const business = businessUnits.find((item) => Number(item.intBusinessUnitId) === Number(elementForm.intBusinessUnitId));
    const payload = {
      ...elementForm,
      intPayrollElementId: toNumber(elementForm.intPayrollElementId),
      intBusinessUnitId: toNumber(elementForm.intBusinessUnitId)
    };
    try {
      if (!store.isDemoMode) {
        await api("/Payroll/CreateNUpdatePayrollElement", { method: "POST", body: JSON.stringify(payload) });
        await store.reload();
      } else {
        store.localUpsert(
          "payrollElements",
          { ...payload, strBusinessUnitName: business?.strBusinessUnitName || "", isActive: true },
          "intPayrollElementId"
        );
      }
      setElementForm(emptyElement);
      store.setNotice("Payroll element saved");
    } catch (error) {
      store.setNotice(error.message);
    }
  }

  async function removeElement(id) {
    try {
      if (!store.isDemoMode) {
        await api(`/Payroll/DeletePayrollElement?elementId=${id}`, { method: "POST" });
        await store.reload();
      } else {
        store.localDelete("payrollElements", "intPayrollElementId", id);
      }
      store.setNotice("Payroll element removed");
    } catch (error) {
      store.setNotice(error.message);
    }
  }

  async function saveBonus(event) {
    event.preventDefault();
    const errors = collectRequiredErrors([
      { label: "Bonus name", value: bonusForm.strBonusSetupName?.trim() },
      { label: "Business unit", value: bonusForm.intBusinessUnitId },
      { label: "Percentage", value: bonusForm.numPercentage }
    ]);

    if (isBlank(bonusForm.intDepartmentId) && isBlank(bonusForm.intEmployementTypeId) && isBlank(bonusForm.intServiceLengthMonths)) {
      errors.push("Department, employment type, or service months");
    }

    if (errors.length > 0) {
      setFormErrors((current) => ({ ...current, bonus: errors }));
      return;
    }

    setFormErrors((current) => ({ ...current, bonus: [] }));
    const department = departments.find((item) => Number(item.intDepartmentId) === Number(bonusForm.intDepartmentId));
    const employment = employmentTypes.find((item) => Number(item.intEmployementId) === Number(bonusForm.intEmployementTypeId));
    const business = businessUnits.find((item) => Number(item.intBusinessUnitId) === Number(bonusForm.intBusinessUnitId));
    const payload = {
      ...bonusForm,
      intBonusSetypId: toNumber(bonusForm.intBonusSetypId),
      intBusinessUnitId: toNumber(bonusForm.intBusinessUnitId),
      intDepartmentId: toNullableNumber(bonusForm.intDepartmentId),
      intEmployementTypeId: toNullableNumber(bonusForm.intEmployementTypeId),
      intServiceLengthMonths: toNullableNumber(bonusForm.intServiceLengthMonths),
      numPercentage: toNumber(bonusForm.numPercentage)
    };
    try {
      if (!store.isDemoMode) {
        await api("/Bonus/CreateNUpdateBonus", { method: "POST", body: JSON.stringify(payload) });
        await store.reload();
      } else {
        store.localUpsert(
          "bonuses",
          {
            ...payload,
            strBusinessUnitName: business?.strBusinessUnitName || "",
            strDepartmentName: department?.strDepartmentName || "",
            strEmployementType: employment?.strEmployementName || "",
            isACTIVE: true
          },
          "intBonusSetypId"
        );
      }
      setBonusForm(emptyBonus);
      store.setNotice("Bonus rule saved");
    } catch (error) {
      store.setNotice(error.message);
    }
  }

  async function saveTransfer(event) {
    event.preventDefault();
    const employee = employees.find((item) => Number(item.intEmployeeBasicInfoId) === Number(transferForm.intEmployeeId));
    const businessUnitId = transferForm.intBusinessUnitId || employee?.intBusinessUnitId || selectedBusinessUnitId;
    const errors = collectRequiredErrors([
      { label: "Employee", value: transferForm.intEmployeeId },
      { label: "Business unit", value: businessUnitId }
    ]);

    if (!transferForm.isTransfer && !transferForm.isPromotion) {
      errors.push("Movement type");
    }

    if (transferForm.isTransfer && isBlank(transferForm.intNewDepartmenId)) {
      errors.push("New department");
    }

    if (transferForm.isTransfer && isBlank(transferForm.intOldDepartmentId || employee?.intDepartmentId)) {
      errors.push("Old department");
    }

    if (transferForm.isPromotion && isBlank(transferForm.intNewDesignationId)) {
      errors.push("New designation");
    }

    if (transferForm.isPromotion && isBlank(transferForm.intOldDesignationId || employee?.intDesignationId)) {
      errors.push("Old designation");
    }

    if (errors.length > 0) {
      setFormErrors((current) => ({ ...current, transfer: errors }));
      return;
    }

    setFormErrors((current) => ({ ...current, transfer: [] }));

    const payload = {
      ...transferForm,
      intEmpTransferNPromotionId: toNumber(transferForm.intEmpTransferNPromotionId),
      intEmployeeId: toNumber(transferForm.intEmployeeId),
      intBusinessUnitId: toNumber(businessUnitId),
      intOldDepartmentId: transferForm.isTransfer ? toNullableNumber(transferForm.intOldDepartmentId || employee?.intDepartmentId) : null,
      intNewDepartmenId: transferForm.isTransfer ? toNumber(transferForm.intNewDepartmenId) : null,
      intOldDesignationId: transferForm.isPromotion ? toNullableNumber(transferForm.intOldDesignationId || employee?.intDesignationId) : null,
      intNewDesignationId: transferForm.isPromotion ? toNumber(transferForm.intNewDesignationId) : null,
      isTransfer: Boolean(transferForm.isTransfer),
      isPromotion: Boolean(transferForm.isPromotion)
    };

    try {
      const oldDepartment = departments.find((item) => Number(item.intDepartmentId) === Number(payload.intOldDepartmentId));
      const newDepartment = departments.find((item) => Number(item.intDepartmentId) === Number(payload.intNewDepartmenId));
      const oldDesignation = designations.find((item) => Number(item.intDesignationId) === Number(payload.intOldDesignationId));
      const newDesignation = designations.find((item) => Number(item.intDesignationId) === Number(payload.intNewDesignationId));
      const business = businessUnits.find((item) => Number(item.intBusinessUnitId) === Number(payload.intBusinessUnitId));
      const historyRow = {
        ...payload,
        strEmployeeName: employee?.strEmployeeName || "",
        strBusinessUnitName: business?.strBusinessUnitName || "",
        strOldDepartmentName: oldDepartment?.strDepartmentName || "",
        strNewDepartmentName: newDepartment?.strDepartmentName || "",
        strOldDesignationName: oldDesignation?.strDesignationName || "",
        strNewDesignationName: newDesignation?.strDesignationName || "",
        strCreatedAt: new Date().toLocaleDateString()
      };

      if (!store.isDemoMode) {
        await api("/Employee/CreateNUpdateEmpTransferNPromoton", { method: "POST", body: JSON.stringify(payload) });
        store.localInsert("transferHistory", historyRow, "intEmpTransferNPromotionId");
        store.reload();
      } else {
        store.localInsert("transferHistory", historyRow, "intEmpTransferNPromotionId");
      }
      setTransferForm(emptyTransfer);
      store.setNotice("Transfer/promotion saved");
    } catch (error) {
      store.setNotice(error.message);
    }
  }

  if (!auth) {
    return <AuthView onAuthenticate={authenticate} />;
  }

  return (
    <div className="app-shell">
      <aside className="side-rail">
        <div className="brand-lockup">
          <img className="brand-logo" src={hrmLogo} alt="HRM logo" />
          <div>
            <span>Your Management Our Solution</span>
          </div>
        </div>

        <nav aria-label="Primary">
          {navItems.map((item) => {
            const Icon = item.icon;
            return (
              <button
                className={active === item.id ? "nav-item active" : "nav-item"}
                key={item.id}
                onClick={() => setActive(item.id)}
                type="button"
              >
                <Icon size={18} />
                <span>{item.label}</span>
              </button>
            );
          })}
        </nav>
      </aside>

      <main className="main-panel">
        <header className="top-bar">
          <div>
            <h1>{navItems.find((item) => item.id === active)?.label}</h1>
          </div>
          <div className="status-cluster">
            <span className="status-pill live">
              <UserRound size={15} />
              {auth.fullName || auth.email}
            </span>
            <span className={store.isDemoMode ? "status-pill demo" : "status-pill live"}>
              {store.loading ? <Loader2 size={15} className="spin" /> : <Check size={15} />}
              {store.notice}
            </span>
            <button className="icon-button" onClick={store.reload} type="button" title="Refresh data">
              <CalendarClock size={18} />
            </button>
            <button className="icon-button" onClick={logout} type="button" title="Sign out">
              <LogOut size={18} />
            </button>
          </div>
        </header>

        {active === "dashboard" && (
          <Dashboard
            activeCount={activeCount}
            totalSalary={totalSalary}
            payrollPolicies={payrollPolicies}
            payrollElements={payrollElements}
            bonuses={bonuses}
            employees={filteredEmployees}
            setActive={setActive}
          />
        )}

        {active === "employees" && (
          <EmployeesView
            employees={filteredEmployees}
            searchText={searchText}
            setSearchText={setSearchText}
            employeeForm={employeeForm}
            setEmployeeForm={setEmployeeForm}
            errors={formErrors.employee}
            saveEmployee={saveEmployee}
            removeEmployee={removeEmployee}
            businessUnits={businessUnits}
            departments={departments}
            designations={designations}
            employmentTypes={employmentTypes}
          />
        )}

        {active === "movement" && (
          <MovementView
            employees={employees}
            departments={departments}
            designations={designations}
            businessUnits={businessUnits}
            transferForm={transferForm}
            setTransferForm={setTransferForm}
            errors={formErrors.transfer}
            saveTransfer={saveTransfer}
            transferHistory={transferHistory}
          />
        )}

        {active === "setup" && (
          <SetupView
            setupDraft={setupDraft}
            setSetupDraft={setSetupDraft}
            setupEdit={setupEdit}
            saveSetup={saveSetup}
            editSetup={editSetup}
            deleteSetup={deleteSetup}
            clearSetupEdit={clearSetupEdit}
            businessUnits={businessUnits}
            departments={departments}
            designations={designations}
            employmentTypes={employmentTypes}
          />
        )}

        {active === "payroll" && (
          <PayrollView
            businessUnits={businessUnits}
            policies={payrollPolicies}
            elements={payrollElements}
            policyForm={policyForm}
            setPolicyForm={setPolicyForm}
            elementForm={elementForm}
            setElementForm={setElementForm}
            policyErrors={formErrors.policy}
            elementErrors={formErrors.element}
            savePolicy={savePolicy}
            saveElement={saveElement}
            removePolicy={removePolicy}
            removeElement={removeElement}
          />
        )}

        {active === "salary" && (
          <SalaryView salaryDetails={salaryDetails} totalSalary={totalSalary} employees={employees} businessUnitSummary={businessUnitSummary} />
        )}

        {active === "bonus" && (
          <BonusView
            bonuses={bonuses}
            bonusForm={bonusForm}
            setBonusForm={setBonusForm}
            errors={formErrors.bonus}
            saveBonus={saveBonus}
            businessUnits={businessUnits}
            departments={departments}
            employmentTypes={employmentTypes}
          />
        )}
      </main>
    </div>
  );
}

function AuthView({ onAuthenticate }) {
  const [mode, setMode] = useState("login");
  const [form, setForm] = useState({ fullName: "", email: "", password: "" });
  const [error, setError] = useState("");
  const [busy, setBusy] = useState(false);
  const isSignup = mode === "signup";

  async function submit(event) {
    event.preventDefault();
    const missing = collectRequiredErrors([
      ...(isSignup ? [{ label: "Full name", value: form.fullName.trim() }] : []),
      { label: "Email", value: form.email.trim() },
      { label: "Password", value: form.password }
    ]);

    if (missing.length > 0) {
      setError(`Complete: ${missing.join(", ")}`);
      return;
    }

    if (isSignup && form.password.length < 6) {
      setError("Password must be at least 6 characters.");
      return;
    }

    setBusy(true);
    setError("");
    try {
      await onAuthenticate(mode, {
        fullName: form.fullName,
        email: form.email,
        password: form.password
      });
    } catch (authError) {
      setError(authError.message);
    } finally {
      setBusy(false);
    }
  }

  return (
    <main className="auth-shell">
      <section className="auth-panel">
        <div className="auth-brand">
          <img className="brand-logo" src={hrmLogo} alt="HRM logo" />
          <div>
            <p className="eyebrow">Your Management</p>
            <p className="eyebrow">Our Solution</p>
          </div>
        </div>

        <div className="auth-tabs" role="tablist">
          <button className={mode === "login" ? "active" : ""} type="button" onClick={() => setMode("login")}>
            <LogIn size={17} /> Login
          </button>
          <button className={mode === "signup" ? "active" : ""} type="button" onClick={() => setMode("signup")}>
            <KeyRound size={17} /> Sign up
          </button>
        </div>

        <form className="auth-form" onSubmit={submit} noValidate>
          {error && (
            <div className="validation-summary" role="alert">
              <strong>Authentication issue</strong>
              <span>{error}</span>
            </div>
          )}
          {isSignup && <TextField label="Full name" value={form.fullName} onChange={(value) => setForm({ ...form, fullName: value })} required />}
          <TextField type="email" label="Email" value={form.email} onChange={(value) => setForm({ ...form, email: value })} required />
          <TextField type="password" label="Password" value={form.password} onChange={(value) => setForm({ ...form, password: value })} required />
          <button className="primary-button auth-submit" type="submit" disabled={busy}>
            {busy ? <Loader2 size={17} className="spin" /> : isSignup ? <KeyRound size={17} /> : <LogIn size={17} />}
            {isSignup ? "Create account" : "Login"}
          </button>
        </form>
      </section>
    </main>
  );
}

function Dashboard({ activeCount, totalSalary, payrollPolicies, payrollElements, bonuses, employees, setActive }) {
  const recent = employees.slice(0, 4);
  return (
    <section className="screen-grid dashboard-grid">
      <Metric icon={UsersRound} label="Active employees" value={activeCount} tone="green" />
      <Metric icon={CircleDollarSign} label="Assigned salary" value={`$${totalSalary.toLocaleString()}`} tone="blue" />
      <Metric icon={ClipboardList} label="Payroll policies" value={payrollPolicies.length} tone="amber" />
      <Metric icon={Gift} label="Bonus rules" value={bonuses.length} tone="rose" />

      <section className="workspace-band dashboard-flow">
        <div className="section-heading">
          <div>
            <p className="eyebrow">Workflow</p>
            <h2>HR operations lane</h2>
          </div>
        </div>
        <div className="lane">
          {[
            ["Setup", "Business units and master data", Building2, "setup"],
            ["Employees", "Employee records and search", UserRound, "employees"],
            ["Transfer", "Movement and promotion history", ArrowRightLeft, "movement"],
            ["Payroll", "Policies and salary elements", WalletCards, "payroll"],
            ["Salary", "Gross salary overview", Banknote, "salary"]
          ].map(([title, detail, Icon, target]) => (
            <button className="lane-step" key={title} onClick={() => setActive(target)} type="button">
              <Icon size={20} />
              <span>
                <strong>{title}</strong>
                <small>{detail}</small>
              </span>
              <ChevronRight size={17} />
            </button>
          ))}
        </div>
      </section>

      <section className="workspace-band">
        <div className="section-heading">
          <div>
            <p className="eyebrow">Recently viewed</p>
            <h2>Employee desk</h2>
          </div>
        </div>
        <div className="compact-list">
          {recent.map((employee) => (
            <div className="person-row" key={employee.strEmployeeCode}>
              <span className="avatar">{employee.strEmployeeName?.slice(0, 1) || "E"}</span>
              <div>
                <strong>{employee.strEmployeeName}</strong>
                <small>{employee.strDepartmentName} / {employee.strDesignationName}</small>
              </div>
              <span className="tag">{employee.strEmployeeCode}</span>
            </div>
          ))}
        </div>
      </section>
    </section>
  );
}

function EmployeesView(props) {
  return (
    <section className="screen-grid two-column">
      <form className="workspace-band form-panel" onSubmit={props.saveEmployee} noValidate>
        <FormTitle icon={UserRound} title={props.employeeForm.intEmployeeBasicInfoId ? "Edit employee" : "New employee"} />
        <ValidationSummary errors={props.errors} />
        <div className="form-grid">
          <TextField label="Code" value={props.employeeForm.strEmployeeCode} onChange={(value) => props.setEmployeeForm({ ...props.employeeForm, strEmployeeCode: value })} required />
          <TextField label="Full name" value={props.employeeForm.strEmployeeName} onChange={(value) => props.setEmployeeForm({ ...props.employeeForm, strEmployeeName: value })} required />
          <SelectField label="Business unit" value={props.employeeForm.intBusinessUnitId} onChange={(value) => props.setEmployeeForm({ ...props.employeeForm, intBusinessUnitId: value })} options={props.businessUnits.map((item) => [item.intBusinessUnitId, item.strBusinessUnitName])} required />
          <SelectField label="Department" value={props.employeeForm.intDepartmentId} onChange={(value) => props.setEmployeeForm({ ...props.employeeForm, intDepartmentId: value })} options={props.departments.map((item) => [item.intDepartmentId, item.strDepartmentName])} required />
          <SelectField label="Designation" value={props.employeeForm.intDesignationId} onChange={(value) => props.setEmployeeForm({ ...props.employeeForm, intDesignationId: value })} options={props.designations.map((item) => [item.intDesignationId, item.strDesignationName])} required />
          <SelectField label="Employment type" value={props.employeeForm.intEmploymentTypeId} onChange={(value) => props.setEmployeeForm({ ...props.employeeForm, intEmploymentTypeId: value })} options={props.employmentTypes.map((item) => [item.intEmployementId, item.strEmployementName])} required />
          <SelectField label="Gender" value={props.employeeForm.strGender} onChange={(value) => props.setEmployeeForm({ ...props.employeeForm, strGender: value })} options={["Female", "Male", "Other"].map((item) => [item, item])} />
          <TextField label="Blood group" value={props.employeeForm.strBloodGroup} onChange={(value) => props.setEmployeeForm({ ...props.employeeForm, strBloodGroup: value })} />
          <TextField type="date" label="Joining date" value={props.employeeForm.dteJoiningDate} onChange={(value) => props.setEmployeeForm({ ...props.employeeForm, dteJoiningDate: value })} required />
        </div>
        <div className="form-actions">
          <button className="primary-button" type="submit"><Plus size={17} /> Save employee</button>
          <button className="ghost-button" type="button" onClick={() => props.setEmployeeForm(emptyEmployee)}><X size={17} /> Clear</button>
        </div>
      </form>

      <section className="workspace-band table-panel">
        <div className="section-heading">
          <div>
            <p className="eyebrow">Company directory</p>
            <h2>All employees ({props.employees.length})</h2>
          </div>
          <label className="search-box">
            <Search size={17} />
            <input value={props.searchText} onChange={(event) => props.setSearchText(event.target.value)} placeholder="Search employees" />
          </label>
        </div>
        <DataTable
          columns={["Code", "Name", "Business unit", "Department", "Designation", "Type", "Actions"]}
          rows={props.employees.map((employee) => [
            employee.strEmployeeCode,
            employee.strEmployeeName,
            employee.strBusinessUnitName,
            employee.strDepartmentName,
            employee.strDesignationName,
            employee.strEmploymentTypeName,
            <ActionSet
              key={employee.strEmployeeCode}
              onEdit={() => props.setEmployeeForm({
                ...emptyEmployee,
                intEmployeeBasicInfoId: employee.intEmployeeBasicInfoId || 0,
                strEmployeeCode: employee.strEmployeeCode,
                strEmployeeName: employee.strEmployeeName,
                intBusinessUnitId: employee.intBusinessUnitId || "",
                intDepartmentId: employee.intDepartmentId || "",
                intDesignationId: employee.intDesignationId || "",
                intEmploymentTypeId: employee.intEmployementTyperId || "",
                strGender: employee.strGender || "",
                strBloodGroup: employee.strBloodGroup || "",
                dteJoiningDate: String(employee.dteJoiningDate || "").slice(0, 10)
              })}
              onDelete={() => props.removeEmployee(employee)}
            />
          ])}
        />
      </section>
    </section>
  );
}

function MovementView({ employees, departments, designations, businessUnits, transferForm, setTransferForm, saveTransfer, transferHistory, errors }) {
  function selectEmployee(employeeId) {
    const employee = employees.find((item) => Number(item.intEmployeeBasicInfoId) === Number(employeeId));
    setTransferForm({
      ...transferForm,
      intEmployeeId: employeeId,
      intBusinessUnitId: employee?.intBusinessUnitId || "",
      intOldDepartmentId: employee?.intDepartmentId || "",
      intOldDesignationId: employee?.intDesignationId || "",
      intNewDepartmenId: "",
      intNewDesignationId: ""
    });
  }

  const oldDepartmentName = departments.find((item) => Number(item.intDepartmentId) === Number(transferForm.intOldDepartmentId))?.strDepartmentName || "No Selection";
  const oldDesignationName = designations.find((item) => Number(item.intDesignationId) === Number(transferForm.intOldDesignationId))?.strDesignationName || "No Selection";

  return (
    <section className="screen-grid two-column">
      <form className="workspace-band form-panel" onSubmit={saveTransfer} noValidate>
        <FormTitle icon={ArrowRightLeft} title="Transfer or promotion" />
        <ValidationSummary errors={errors} />
        <div className="movement-choice">
          <label className={transferForm.isTransfer ? "choice-card active" : "choice-card"}>
            <input
              type="checkbox"
              checked={transferForm.isTransfer}
              onChange={(event) => setTransferForm({ ...transferForm, isTransfer: event.target.checked })}
            />
            <Building2 size={18} />
            <span>
              <strong>Department transfer</strong>
              <small>Move an employee to a new department.</small>
            </span>
          </label>
          <label className={transferForm.isPromotion ? "choice-card active" : "choice-card"}>
            <input
              type="checkbox"
              checked={transferForm.isPromotion}
              onChange={(event) => setTransferForm({ ...transferForm, isPromotion: event.target.checked })}
            />
            <BadgeDollarSign size={18} />
            <span>
              <strong>Designation promotion</strong>
              <small>Promote or change the employee designation.</small>
            </span>
          </label>
        </div>

        <div className="form-grid">
          <SelectField
            label="Employee"
            value={transferForm.intEmployeeId}
            onChange={selectEmployee}
            options={employees.map((item) => [item.intEmployeeBasicInfoId, `${item.strEmployeeCode} - ${item.strEmployeeName}`])}
            required
          />
          <SelectField
            label="Business unit"
            value={transferForm.intBusinessUnitId}
            onChange={(value) => setTransferForm({ ...transferForm, intBusinessUnitId: value })}
            options={businessUnits.map((item) => [item.intBusinessUnitId, item.strBusinessUnitName])}
            required
          />
          <ReadOnlyField label="Old department" value={oldDepartmentName} />
          <SelectField
            label="New department"
            value={transferForm.intNewDepartmenId}
            onChange={(value) => setTransferForm({ ...transferForm, intNewDepartmenId: value })}
            options={departments.map((item) => [item.intDepartmentId, item.strDepartmentName])}
            required={transferForm.isTransfer}
          />
          <ReadOnlyField label="Old designation" value={oldDesignationName} />
          <SelectField
            label="New designation"
            value={transferForm.intNewDesignationId}
            onChange={(value) => setTransferForm({ ...transferForm, intNewDesignationId: value })}
            options={designations.map((item) => [item.intDesignationId, item.strDesignationName])}
            required={transferForm.isPromotion}
          />
        </div>
        <div className="form-actions">
          <button className="primary-button" type="submit"><ArrowRightLeft size={17} /> Save movement</button>
          <button className="ghost-button" type="button" onClick={() => setTransferForm(emptyTransfer)}><X size={17} /> Clear</button>
        </div>
      </form>

      <section className="workspace-band table-panel">
        <div className="section-heading">
          <div>
            <p className="eyebrow">Movement log</p>
            <h2>Transfer and promotion history</h2>
          </div>
        </div>
        <DataTable
          columns={["Employee", "Type", "From", "To", "Created"]}
          rows={transferHistory.map((item) => [
            item.strEmployeeName,
            item.isTransfer && item.isPromotion ? "Transfer + Promotion" : item.isTransfer ? "Transfer" : "Promotion",
            item.isTransfer ? item.strOldDepartmentName : item.strOldDesignationName,
            item.isTransfer ? item.strNewDepartmentName : item.strNewDesignationName,
            item.strCreatedAt
          ])}
        />
      </section>
    </section>
  );
}

function SetupView({ setupDraft, setSetupDraft, setupEdit, saveSetup, editSetup, deleteSetup, clearSetupEdit, businessUnits, departments, designations, employmentTypes }) {
  const groups = [
    ["businessUnit", "Business units", Building2, businessUnits, "intBusinessUnitId", "strBusinessUnitName"],
    ["department", "Departments", BriefcaseBusiness, departments, "intDepartmentId", "strDepartmentName"],
    ["designation", "Designations", BadgeDollarSign, designations, "intDesignationId", "strDesignationName"],
    ["employmentType", "Employment types", ClipboardList, employmentTypes, "intEmployementId", "strEmployementName"]
  ];

  return (
    <section className="setup-grid">
      {groups.map(([key, title, Icon, items, idKey, labelKey]) => (
        <section className="workspace-band setup-panel" key={key}>
          <div className="setup-heading">
            <FormTitle icon={Icon} title={title} />
          </div>
          <div className="inline-create">
            <input value={setupDraft[key]} onChange={(event) => setSetupDraft({ ...setupDraft, [key]: event.target.value })} placeholder={`New ${title.toLowerCase()}`} />
            <button className="icon-button filled" type="button" onClick={() => saveSetup(key)} title={setupEdit.type === key ? `Update ${title}` : `Add ${title}`}>
              {setupEdit.type === key ? <Check size={18} /> : <Plus size={18} />}
            </button>
            {setupEdit.type === key && (
              <button className="icon-button" type="button" onClick={() => clearSetupEdit(key)} title="Cancel edit">
                <X size={17} />
              </button>
            )}
          </div>
          <div className="setup-list">
            {items.map((item) => (
              <div className="setup-row" key={item[idKey]}>
                <span>{item[labelKey]}</span>
                <div className="action-set">
                  <button className="icon-button" type="button" onClick={() => editSetup(key, item, idKey, labelKey)} title={`Edit ${item[labelKey]}`}>
                    <Edit3 size={16} />
                  </button>
                  <button className="icon-button danger" type="button" onClick={() => deleteSetup(key, item[idKey])} title={`Delete ${item[labelKey]}`}>
                    <Trash2 size={16} />
                  </button>
                </div>
              </div>
            ))}
          </div>
        </section>
      ))}
    </section>
  );
}

function PayrollView(props) {
  return (
    <section className="screen-grid two-column">
      <section className="workspace-band form-panel">
        <form onSubmit={props.savePolicy} noValidate>
          <FormTitle icon={ClipboardList} title={props.policyForm.intPayrollPolicyId ? "Edit policy" : "Payroll policy"} />
          <ValidationSummary errors={props.policyErrors} />
          <div className="form-grid single">
            <TextField label="Policy name" value={props.policyForm.strPayrollPolicyName} onChange={(value) => props.setPolicyForm({ ...props.policyForm, strPayrollPolicyName: value })} required />
            <SelectField label="Business unit" value={props.policyForm.intBusinessUnitId} onChange={(value) => props.setPolicyForm({ ...props.policyForm, intBusinessUnitId: value })} options={props.businessUnits.map((item) => [item.intBusinessUnitId, item.strBusinessUnitName])} required />
            <TextField type="number" label="Salary days" value={props.policyForm.intGrossSalaryDevidedByDays} onChange={(value) => props.setPolicyForm({ ...props.policyForm, intGrossSalaryDevidedByDays: value })} required />
          </div>
          <div className="form-actions"><button className="primary-button" type="submit"><Plus size={17} /> Save policy</button></div>
        </form>

        <form onSubmit={props.saveElement} className="stacked-form" noValidate>
          <FormTitle icon={WalletCards} title={props.elementForm.intPayrollElementId ? "Edit element" : "Payroll element"} />
          <ValidationSummary errors={props.elementErrors} />
          <div className="form-grid single">
            <TextField label="Element name" value={props.elementForm.strPayrollElementName} onChange={(value) => props.setElementForm({ ...props.elementForm, strPayrollElementName: value })} required />
            <SelectField label="Business unit" value={props.elementForm.intBusinessUnitId} onChange={(value) => props.setElementForm({ ...props.elementForm, intBusinessUnitId: value })} options={props.businessUnits.map((item) => [item.intBusinessUnitId, item.strBusinessUnitName])} required />
            <label className="toggle-line"><input type="checkbox" checked={props.elementForm.isSalaryElement} onChange={(event) => props.setElementForm({ ...props.elementForm, isSalaryElement: event.target.checked })} /> Salary element</label>
            <label className="toggle-line"><input type="checkbox" checked={props.elementForm.isBasicElement} onChange={(event) => props.setElementForm({ ...props.elementForm, isBasicElement: event.target.checked })} /> Addition/deduction element</label>
          </div>
          <div className="form-actions"><button className="primary-button" type="submit"><Plus size={17} /> Save element</button></div>
        </form>
      </section>

      <section className="workspace-band table-panel">
        <div className="section-heading">
          <div>
            <p className="eyebrow">Rules</p>
            <h2>Payroll configuration</h2>
          </div>
        </div>
        <DataTable
          columns={["Policy", "Business unit", "Days", "Actions"]}
          rows={props.policies.map((policy) => [
            policy.strPayrollPolicyName,
            policy.strBusinessUnitName,
            policy.intGrossSalaryDevidedByDays || "Actual",
            <ActionSet
              key={policy.intPayrollPolicyId}
              onEdit={() => props.setPolicyForm({ ...emptyPolicy, ...policy, intBusinessUnitId: policy.intBusinessUnitId || "" })}
              onDelete={() => props.removePolicy(policy.intPayrollPolicyId)}
            />
          ])}
        />
        <DataTable
          columns={["Element", "Business unit", "Type", "Actions"]}
          rows={props.elements.map((element) => [
            element.strPayrollElementName,
            element.strBusinessUnitName,
            element.isSalaryElement ? "Salary" : "Basic",
            <ActionSet
              key={element.intPayrollElementId}
              onEdit={() => props.setElementForm({ ...emptyElement, ...element, intBusinessUnitId: element.intBusinessUnitId || "" })}
              onDelete={() => props.removeElement(element.intPayrollElementId)}
            />
          ])}
        />
      </section>
    </section>
  );
}

function SalaryView({ salaryDetails, totalSalary, employees, businessUnitSummary }) {
  async function generateSalary() {
    try {
      await api(`/Salary/SalaryGenerate?intYearId=${new Date().getFullYear()}&intMonthId=${new Date().getMonth() + 1}`, { method: "POST" });
    } catch {
      return null;
    }
    return null;
  }

  return (
    <section className="screen-grid">
      <div className="metrics-row">
        <Metric icon={Banknote} label="Gross salary" value={`$${totalSalary.toLocaleString()}`} tone="green" />
        <Metric icon={UsersRound} label="Employees tracked" value={employees.length} tone="blue" />
        <Metric icon={Building2} label="Business units" value={businessUnitSummary} tone="amber" />
      </div>
      <section className="workspace-band table-panel">
        <div className="section-heading">
          <div>
            <p className="eyebrow">Salary desk</p>
            <h2>Assigned salary details</h2>
          </div>
          <button className="primary-button" type="button" onClick={generateSalary}><Banknote size={17} /> Generate</button>
        </div>
        <DataTable
          columns={["Employee", "Department", "Designation", "Group", "Gross salary"]}
          rows={salaryDetails.map((salary) => [
            salary.strEmployeeName,
            salary.strDepartmentName,
            salary.strDesignationName,
            salary.strPayrollGroupHeader,
            `$${Number(salary.numGrossSalary || 0).toLocaleString()}`
          ])}
        />
      </section>
    </section>
  );
}

function BonusView(props) {
  return (
    <section className="screen-grid two-column">
      <form className="workspace-band form-panel" onSubmit={props.saveBonus} noValidate>
        <FormTitle icon={Gift} title={props.bonusForm.intBonusSetypId ? "Edit bonus" : "Bonus setup"} />
        <ValidationSummary errors={props.errors} />
        <div className="form-grid">
          <TextField label="Bonus name" value={props.bonusForm.strBonusSetupName} onChange={(value) => props.setBonusForm({ ...props.bonusForm, strBonusSetupName: value })} required />
          <SelectField label="Business unit" value={props.bonusForm.intBusinessUnitId} onChange={(value) => props.setBonusForm({ ...props.bonusForm, intBusinessUnitId: value })} options={props.businessUnits.map((item) => [item.intBusinessUnitId, item.strBusinessUnitName])} required />
          <SelectField label="Department" value={props.bonusForm.intDepartmentId} onChange={(value) => props.setBonusForm({ ...props.bonusForm, intDepartmentId: value })} options={props.departments.map((item) => [item.intDepartmentId, item.strDepartmentName])} />
          <SelectField label="Employment type" value={props.bonusForm.intEmployementTypeId} onChange={(value) => props.setBonusForm({ ...props.bonusForm, intEmployementTypeId: value })} options={props.employmentTypes.map((item) => [item.intEmployementId, item.strEmployementName])} />
          <TextField type="number" label="Service months" value={props.bonusForm.intServiceLengthMonths} onChange={(value) => props.setBonusForm({ ...props.bonusForm, intServiceLengthMonths: value })} />
          <TextField type="number" label="Percentage" value={props.bonusForm.numPercentage} onChange={(value) => props.setBonusForm({ ...props.bonusForm, numPercentage: value })} required />
        </div>
        <div className="form-actions"><button className="primary-button" type="submit"><Gift size={17} /> Save bonus</button></div>
      </form>

      <section className="workspace-band table-panel">
        <div className="section-heading">
          <div>
            <p className="eyebrow">Eligibility</p>
            <h2>Bonus rules</h2>
          </div>
        </div>
        <DataTable
          columns={["Name", "Department", "Type", "Service", "Percent", "Actions"]}
          rows={props.bonuses.map((bonus) => [
            bonus.strBonusSetupName,
            bonus.strDepartmentName,
            bonus.strEmployementType,
            bonus.intServiceLengthMonths ? `${bonus.intServiceLengthMonths} months` : "Any",
            `${bonus.numPercentage}%`,
            <button className="icon-button" key={bonus.intBonusSetypId} type="button" title="Edit bonus" onClick={() => props.setBonusForm({ ...emptyBonus, ...bonus, intBusinessUnitId: bonus.intBusinessUnitId || "" })}>
              <Edit3 size={17} />
            </button>
          ])}
        />
      </section>
    </section>
  );
}

function Metric({ icon: Icon, label, value, tone }) {
  return (
    <section className={`metric ${tone}`}>
      <Icon size={21} />
      <div>
        <span>{label}</span>
        <strong>{value}</strong>
      </div>
    </section>
  );
}

function FormTitle({ icon: Icon, title }) {
  return (
    <div className="form-title">
      <Icon size={19} />
      <h2>{title}</h2>
    </div>
  );
}

function TextField({ label, value, onChange, type = "text", required = false }) {
  return (
    <label className="field">
      <span>{label}{required ? " *" : ""}</span>
      <input type={type} value={value ?? ""} onChange={(event) => onChange(event.target.value)} required={required} />
    </label>
  );
}

function SelectField({ label, value, onChange, options, required = false }) {
  return (
    <label className="field">
      <span>{label}{required ? " *" : ""}</span>
      <select value={value ?? ""} onChange={(event) => onChange(event.target.value)}>
        <option value="">No Selection</option>
        {options.map(([optionValue, optionLabel]) => (
          <option value={optionValue} key={`${optionValue}-${optionLabel}`}>
            {optionLabel}
          </option>
        ))}
      </select>
    </label>
  );
}

function ReadOnlyField({ label, value }) {
  return (
    <label className="field">
      <span>{label}</span>
      <input value={value || "No Selection"} readOnly className="readonly-input" />
    </label>
  );
}

function ValidationSummary({ errors = [] }) {
  if (!errors.length) return null;

  return (
    <div className="validation-summary" role="alert">
      <strong>Before saving, complete:</strong>
      <span>{errors.join(", ")}</span>
    </div>
  );
}

function DataTable({ columns, rows }) {
  return (
    <div className="table-wrap">
      <table>
        <thead>
          <tr>{columns.map((column) => <th key={column}>{column}</th>)}</tr>
        </thead>
        <tbody>
          {rows.length === 0 ? (
            <tr><td colSpan={columns.length} className="empty-cell">No records found</td></tr>
          ) : rows.map((row, index) => (
            <tr key={index}>{row.map((cell, cellIndex) => <td key={cellIndex}>{cell}</td>)}</tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

function ActionSet({ onEdit, onDelete }) {
  return (
    <div className="action-set">
      <button className="icon-button" type="button" title="Edit" onClick={onEdit}><Edit3 size={16} /></button>
      <button className="icon-button danger" type="button" title="Delete" onClick={onDelete}><Trash2 size={16} /></button>
    </div>
  );
}

createRoot(document.getElementById("root")).render(<App />);
