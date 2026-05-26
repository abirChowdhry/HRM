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
const AUTH_SESSION_MS = 60 * 60 * 1000;

function getTokenExpiry(token) {
  try {
    const payload = JSON.parse(atob(token.split(".")[1] || ""));
    return payload.exp ? payload.exp * 1000 : null;
  } catch {
    return null;
  }
}

function getStoredAuth() {
  try {
    const auth = JSON.parse(localStorage.getItem(AUTH_KEY) || "null");
    if (!auth?.token) return null;

    const jwtExpiry = getTokenExpiry(auth.token);
    const expiry = auth.expiresAt || jwtExpiry;
    if (expiry && Date.now() >= expiry) {
      localStorage.removeItem(AUTH_KEY);
      return null;
    }

    return auth;
  } catch {
    localStorage.removeItem(AUTH_KEY);
    return null;
  }
}

function createSession(response) {
  const now = Date.now();
  const jwtExpiry = getTokenExpiry(response.token);
  const sessionExpiry = now + AUTH_SESSION_MS;
  return {
    ...response,
    issuedAt: now,
    expiresAt: jwtExpiry ? Math.min(sessionExpiry, jwtExpiry) : sessionExpiry
  };
}

const emptyData = {
  businessUnits: [],
  departments: [],
  designations: [],
  employmentTypes: [],
  employees: [],
  payrollGroups: [],
  payrollGroupRows: {},
  salaryCandidates: [],
  salaryDetails: [],
  salaryAdjustments: [],
  salaryGeneration: [],
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

const emptyPayrollGroup = {
  intPayrollGroupHeaderId: 0,
  strPayrollGroupHeaderTitle: "",
  intBusinessUnitId: "",
  intPayrollPolicyId: null,
  intCreatedBy: 1,
  intUpdatedBy: 1,
  payrollGroupRowList: [
    { intPayrollGroupRowId: 0, intPayrollElementTypeId: 1, strPayrollElementName: "Basic salary", numNumberOfPercent: 60, isActive: true },
    { intPayrollGroupRowId: 0, intPayrollElementTypeId: 2, strPayrollElementName: "House allowance", numNumberOfPercent: 25, isActive: true },
    { intPayrollGroupRowId: 0, intPayrollElementTypeId: 3, strPayrollElementName: "Other allowance", numNumberOfPercent: 15, isActive: true }
  ]
};

const emptySalaryAssign = {
  intSalaryAssignHeaderId: 0,
  intPayrollGroupHeaderId: "",
  intEmployeeId: "",
  intBusinessUnitId: "",
  numGrossSalary: "",
  numNetGrossSalary: "",
  intCreateBy: 1,
  intUpdateBy: 1
};

const emptyAdjustment = {
  intSalaryAdditionAndDeductionId: 0,
  intBusinessUnitId: "",
  intEmployeeId: "",
  intYear: new Date().getFullYear(),
  intMonth: new Date().getMonth() + 1,
  isAddition: true,
  isDeduction: false,
  intAdditionNdeductionTypeId: "",
  numAmount: "",
  intCreatedBy: 1,
  intUpdatedBy: 1
};

const adjustmentTypes = [
  [1, "Allowance"],
  [2, "Overtime"],
  [3, "Tax"],
  [4, "Advance"],
  [5, "Penalty"]
];

const months = [
  [1, "January"],
  [2, "February"],
  [3, "March"],
  [4, "April"],
  [5, "May"],
  [6, "June"],
  [7, "July"],
  [8, "August"],
  [9, "September"],
  [10, "October"],
  [11, "November"],
  [12, "December"]
];

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
  { id: "salary", label: "Salary", icon: Banknote },
  { id: "payroll", label: "Payroll", icon: WalletCards }
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

      const [groupResponses, salaryCandidateResponses, salaryDetailResponses, salaryAdjustments] = await Promise.all([
        Promise.all(businessUnitIds.map((businessUnitId) => api(`/Payroll/PayrollHeaderLanding?businessUnitId=${businessUnitId}`, { method: "POST" }))),
        Promise.all(businessUnitIds.map((businessUnitId) => api(`/Salary/SalaryAssignLanding?businessUnitId=${businessUnitId}`, { method: "POST" }))),
        Promise.all(businessUnitIds.map((businessUnitId) => api(`/Salary/SalaryDetailsLanding?businessUnitId=${businessUnitId}`, { method: "POST" }))),
        api("/Salary/SalaryAdjustmentLanding", { method: "POST" })
      ]);

      const payrollGroups = groupResponses.flatMap((response) => response || []);
      const salaryCandidates = salaryCandidateResponses.flatMap((response) => response || []);
      const salaryDetails = salaryDetailResponses.flatMap((response) => response || []);
      const rowResponses = await Promise.all(
        payrollGroups.map((group) => api(`/Payroll/PayrollRowLanding?headerId=${group.intPayrollGroupHeaderId}`, { method: "POST" }))
      );
      const payrollGroupRows = payrollGroups.reduce((rowsByGroup, group, index) => ({
        ...rowsByGroup,
        [group.intPayrollGroupHeaderId]: rowResponses[index] || []
      }), {});

      const salaryGeneration = [];

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
        payrollGroups,
        payrollGroupRows,
        salaryCandidates,
        salaryDetails,
        salaryAdjustments,
        salaryGeneration,
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

  function localReplace(collection, rows) {
    setData((current) => ({
      ...current,
      [collection]: rows
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
    localDelete,
    localReplace
  };
}

function App() {
  const [auth, setAuth] = useState(getStoredAuth);
  const store = useHrmData();
  const [active, setActive] = useState("dashboard");
  const [searchText, setSearchText] = useState("");
  const [employeeForm, setEmployeeForm] = useState(emptyEmployee);
  const [payrollGroupForm, setPayrollGroupForm] = useState(emptyPayrollGroup);
  const [salaryAssignForm, setSalaryAssignForm] = useState(emptySalaryAssign);
  const [adjustmentForm, setAdjustmentForm] = useState(emptyAdjustment);
  const [transferForm, setTransferForm] = useState(emptyTransfer);
  const [setupDraft, setSetupDraft] = useState({ businessUnit: "", department: "", designation: "", employmentType: "" });
  const [setupEdit, setSetupEdit] = useState({ type: "", id: "" });
  const [formErrors, setFormErrors] = useState({
    employee: [],
    payrollGroup: [],
    salaryAssign: [],
    adjustment: [],
    transfer: []
  });

  const {
    businessUnits,
    departments,
    designations,
    employmentTypes,
    employees,
    payrollGroups,
    payrollGroupRows,
    salaryCandidates,
    salaryDetails,
    salaryAdjustments,
    salaryGeneration,
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
    const session = createSession(response);
    localStorage.setItem(AUTH_KEY, JSON.stringify(session));
    setAuth(session);
    store.setNotice(`Welcome, ${session.fullName || session.email}`);
    await store.reload();
  }

  function logout(message = "Signed out") {
    localStorage.removeItem(AUTH_KEY);
    setAuth(null);
    setActive("dashboard");
    store.setNotice(message);
  }

  useEffect(() => {
    if (!auth?.expiresAt) return undefined;

    const remaining = auth.expiresAt - Date.now();
    if (remaining <= 0) {
      logout("Session expired. Please login again.");
      return undefined;
    }

    const timeoutId = window.setTimeout(() => {
      logout("Session expired. Please login again.");
    }, remaining);

    return () => window.clearTimeout(timeoutId);
  }, [auth?.expiresAt]);

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

  async function savePayrollGroup(event) {
    event.preventDefault();
    const totalPercent = payrollGroupForm.payrollGroupRowList.reduce((sum, row) => sum + Number(row.numNumberOfPercent || 0), 0);
    const errors = collectRequiredErrors([
      { label: "Structure title", value: payrollGroupForm.strPayrollGroupHeaderTitle?.trim() },
      { label: "Business unit", value: payrollGroupForm.intBusinessUnitId }
    ]);

    if (payrollGroupForm.payrollGroupRowList.some((row) => !row.strPayrollElementName?.trim() || Number(row.numNumberOfPercent || 0) <= 0)) {
      errors.push("Structure rows");
    }

    if (totalPercent !== 100) {
      errors.push("Component percentage must total 100");
    }

    if (errors.length > 0) {
      setFormErrors((current) => ({ ...current, payrollGroup: errors }));
      return;
    }

    setFormErrors((current) => ({ ...current, payrollGroup: [] }));
    const business = businessUnits.find((item) => Number(item.intBusinessUnitId) === Number(payrollGroupForm.intBusinessUnitId));
    const payload = {
      ...payrollGroupForm,
      intPayrollGroupHeaderId: toNumber(payrollGroupForm.intPayrollGroupHeaderId),
      intBusinessUnitId: toNumber(payrollGroupForm.intBusinessUnitId),
      intPayrollPolicyId: null,
      payrollGroupRowList: payrollGroupForm.payrollGroupRowList.map((row, index) => ({
        ...row,
        intPayrollGroupRowId: toNumber(row.intPayrollGroupRowId),
        intPayrollElementTypeId: toNumber(row.intPayrollElementTypeId || index + 1),
        strPayrollElementName: row.strPayrollElementName.trim(),
        numNumberOfPercent: Number(row.numNumberOfPercent || 0),
        isActive: true
      }))
    };

    try {
      if (!store.isDemoMode) {
        await api("/Payroll/PayrollGroupHeaderNRowCreateNUpdate", { method: "POST", body: JSON.stringify(payload) });
        await store.reload();
      } else {
        const isEditing = payload.intPayrollGroupHeaderId > 0;
        const nextId = isEditing ? payload.intPayrollGroupHeaderId : Math.max(0, ...payrollGroups.map((row) => Number(row.intPayrollGroupHeaderId || 0))) + 1;
        store.localUpsert(
          "payrollGroups",
          {
            intPayrollGroupHeaderId: nextId,
            strPayrollGroupHeaderTitle: payload.strPayrollGroupHeaderTitle,
            intBusinessUnitId: payload.intBusinessUnitId,
            strBusinessUnitName: business?.strBusinessUnitName || "",
            strPayrollPolicyName: "",
            isActive: true
          },
          "intPayrollGroupHeaderId"
        );
        store.localReplace("payrollGroupRows", {
          ...payrollGroupRows,
          [nextId]: payload.payrollGroupRowList.map((row, index) => ({
            ...row,
            intPayrollGroupRowId: row.intPayrollGroupRowId || index + 1
          }))
        });
      }
      setPayrollGroupForm(emptyPayrollGroup);
      store.setNotice("Salary structure saved");
    } catch (error) {
      store.setNotice(error.message);
    }
  }

  function updatePayrollGroupRow(index, key, value) {
    setPayrollGroupForm((current) => ({
      ...current,
      payrollGroupRowList: current.payrollGroupRowList.map((row, rowIndex) => (rowIndex === index ? { ...row, [key]: value } : row))
    }));
  }

  function addPayrollGroupRow() {
    setPayrollGroupForm((current) => ({
      ...current,
      payrollGroupRowList: [
        ...current.payrollGroupRowList,
        { intPayrollGroupRowId: 0, intPayrollElementTypeId: current.payrollGroupRowList.length + 1, strPayrollElementName: "", numNumberOfPercent: "", isActive: true }
      ]
    }));
  }

  function removePayrollGroupRow(index) {
    setPayrollGroupForm((current) => ({
      ...current,
      payrollGroupRowList: current.payrollGroupRowList.filter((_, rowIndex) => rowIndex !== index)
    }));
  }

  async function removePayrollGroup(id) {
    try {
      if (!store.isDemoMode) {
        await api(`/Payroll/DeletePayrollHeaderNRow?headerId=${id}`, { method: "POST" });
        await store.reload();
      } else {
        store.localDelete("payrollGroups", "intPayrollGroupHeaderId", id);
      }
      store.setNotice("Salary structure removed");
    } catch (error) {
      store.setNotice(error.message);
    }
  }

  function editPayrollGroup(group) {
    const rows = payrollGroupRows[group.intPayrollGroupHeaderId] || [];
    setPayrollGroupForm({
      ...emptyPayrollGroup,
      intPayrollGroupHeaderId: group.intPayrollGroupHeaderId || 0,
      strPayrollGroupHeaderTitle: group.strPayrollGroupHeaderTitle || "",
      intBusinessUnitId: group.intBusinessUnitId || "",
      intPayrollPolicyId: null,
      intUpdatedBy: 1,
      payrollGroupRowList: rows.length
        ? rows.map((row, index) => ({
            intPayrollGroupRowId: row.intPayrollGroupRowId || 0,
            intPayrollElementTypeId: row.intPayrollElementTypeId || index + 1,
            strPayrollElementName: row.strPayrollElementName || "",
            numNumberOfPercent: row.numNumberOfPercent || "",
            isActive: true
          }))
        : emptyPayrollGroup.payrollGroupRowList
    });
  }

  async function saveSalaryAssignment(event) {
    event.preventDefault();
    const employee = employees.find((item) => Number(item.intEmployeeBasicInfoId) === Number(salaryAssignForm.intEmployeeId));
    const businessUnitId = salaryAssignForm.intBusinessUnitId || employee?.intBusinessUnitId;
    const errors = collectRequiredErrors([
      { label: "Employee", value: salaryAssignForm.intEmployeeId },
      { label: "Business unit", value: businessUnitId },
      { label: "Salary structure", value: salaryAssignForm.intPayrollGroupHeaderId },
      { label: "Gross salary", value: salaryAssignForm.numGrossSalary }
    ]);

    if (Number(salaryAssignForm.numGrossSalary || 0) <= 0) {
      errors.push("Gross salary must be greater than 0");
    }

    if (errors.length > 0) {
      setFormErrors((current) => ({ ...current, salaryAssign: errors }));
      return;
    }

    setFormErrors((current) => ({ ...current, salaryAssign: [] }));
    const payrollGroup = payrollGroups.find((item) => Number(item.intPayrollGroupHeaderId) === Number(salaryAssignForm.intPayrollGroupHeaderId));
    const payload = {
      ...salaryAssignForm,
      intSalaryAssignHeaderId: toNumber(salaryAssignForm.intSalaryAssignHeaderId),
      intPayrollGroupHeaderId: toNumber(salaryAssignForm.intPayrollGroupHeaderId),
      intEmployeeId: toNumber(salaryAssignForm.intEmployeeId),
      intBusinessUnitId: toNumber(businessUnitId),
      numGrossSalary: Number(salaryAssignForm.numGrossSalary || 0),
      numNetGrossSalary: Number(salaryAssignForm.numNetGrossSalary || salaryAssignForm.numGrossSalary || 0),
      intCreateBy: 1,
      intUpdateBy: 1
    };

    try {
      if (!store.isDemoMode) {
        await api("/Salary/SalaryAssign", { method: "POST", body: JSON.stringify(payload) });
        await store.reload();
      } else {
        store.localUpsert(
          "salaryDetails",
          {
            ...payload,
            strEmployeeName: employee?.strEmployeeName || "",
            strBusinessUnitName: employee?.strBusinessUnitName || "",
            strDepartmentName: employee?.strDepartmentName || "",
            strDesignationName: employee?.strDesignationName || "",
            intPayrollGroupHeader: payload.intPayrollGroupHeaderId,
            strPayrollGroupHeader: payrollGroup?.strPayrollGroupHeaderTitle || ""
          },
          "intEmployeeId"
        );
      }
      setSalaryAssignForm(emptySalaryAssign);
      store.setNotice("Salary assigned");
    } catch (error) {
      store.setNotice(error.message);
    }
  }

  async function removeSalaryAssignment(id) {
    try {
      if (!store.isDemoMode) {
        await api(`/Salary/DeleteSalaryAssign?salaryAssignHeaderId=${id}`, { method: "POST" });
        await store.reload();
      } else {
        store.localDelete("salaryDetails", "intSalaryAssignHeaderId", id);
      }
      store.setNotice("Assigned salary removed");
    } catch (error) {
      store.setNotice(error.message);
    }
  }

  async function saveAdjustment(event) {
    event.preventDefault();
    const isEditing = Number(adjustmentForm.intSalaryAdditionAndDeductionId || 0) > 0;
    const isAllBusinessUnits = adjustmentForm.intBusinessUnitId === "all";
    const isAllEmployees = adjustmentForm.intEmployeeId === "all";
    const employee = isAllEmployees ? null : employees.find((item) => Number(item.intEmployeeBasicInfoId) === Number(adjustmentForm.intEmployeeId));
    const errors = collectRequiredErrors([
      { label: "Business unit", value: adjustmentForm.intBusinessUnitId },
      { label: "Employee", value: adjustmentForm.intEmployeeId },
      { label: "Type", value: adjustmentForm.intAdditionNdeductionTypeId },
      { label: "Year", value: adjustmentForm.intYear },
      { label: "Month", value: adjustmentForm.intMonth },
      { label: "Amount", value: adjustmentForm.numAmount }
    ]);

    if (!isAllEmployees && !employee) {
      errors.push("Employee");
    }

    if (Number(adjustmentForm.numAmount || 0) <= 0) {
      errors.push("Amount must be greater than 0");
    }

    if (errors.length > 0) {
      setFormErrors((current) => ({ ...current, adjustment: errors }));
      return;
    }

    setFormErrors((current) => ({ ...current, adjustment: [] }));
    const assignedEmployeeIds = new Set(salaryDetails.map((salary) => Number(salary.intEmployeeId)));
    const targetEmployees = isEditing || !isAllEmployees
      ? [employee].filter(Boolean)
      : employees.filter((item) => assignedEmployeeIds.has(Number(item.intEmployeeBasicInfoId))
        && (isAllBusinessUnits || Number(item.intBusinessUnitId) === Number(adjustmentForm.intBusinessUnitId)));

    if (targetEmployees.length === 0) {
      setFormErrors((current) => ({ ...current, adjustment: ["No assigned employees found for this selection"] }));
      return;
    }

    try {
      if (!store.isDemoMode) {
        await Promise.all(targetEmployees.map((targetEmployee) => api("/Salary/CreateSalaryAdditionNDecduction", {
          method: "POST",
          body: JSON.stringify({
            intSalaryAdditionAndDeductionId: isEditing ? toNumber(adjustmentForm.intSalaryAdditionAndDeductionId) : 0,
            intBusinessUnitId: toNumber(targetEmployee.intBusinessUnitId),
            intEmployeeId: toNumber(targetEmployee.intEmployeeBasicInfoId),
            intYear: toNumber(adjustmentForm.intYear),
            intMonth: toNumber(adjustmentForm.intMonth),
            intAdditionNdeductionTypeId: toNumber(adjustmentForm.intAdditionNdeductionTypeId),
            numAmount: Number(adjustmentForm.numAmount || 0),
            isAddition: Boolean(adjustmentForm.isAddition),
            isDeduction: Boolean(adjustmentForm.isDeduction),
            intCreatedBy: 1,
            intUpdatedBy: 1
          })
        })));
        await store.reload();
      } else {
        targetEmployees.forEach((targetEmployee) => {
          store.localUpsert(
            "salaryAdjustments",
            {
              intSalaryAdditionAndDeductionId: isEditing ? adjustmentForm.intSalaryAdditionAndDeductionId : 0,
              intBusinessUnitId: targetEmployee.intBusinessUnitId,
              strBusinessUnitName: targetEmployee.strBusinessUnitName || "",
              intEmployeeId: targetEmployee.intEmployeeBasicInfoId,
              strEmployeeName: targetEmployee.strEmployeeName || "",
              intYear: toNumber(adjustmentForm.intYear),
              intMonth: toNumber(adjustmentForm.intMonth),
              intAdditionNdeductionTypeId: toNumber(adjustmentForm.intAdditionNdeductionTypeId),
              numAmount: Number(adjustmentForm.numAmount || 0),
              isAddition: Boolean(adjustmentForm.isAddition),
              isDeduction: Boolean(adjustmentForm.isDeduction)
            },
            "intSalaryAdditionAndDeductionId"
          );
        });
      }
      setAdjustmentForm(emptyAdjustment);
      store.setNotice("Salary adjustment saved");
    } catch (error) {
      store.setNotice(error.message);
    }
  }

  async function removeAdjustment(id) {
    try {
      if (!store.isDemoMode) {
        await api(`/Salary/DeleteSalaryAdjustment?adjustmentId=${id}`, { method: "POST" });
        await store.reload();
      } else {
        store.localDelete("salaryAdjustments", "intSalaryAdditionAndDeductionId", id);
      }
      store.setNotice("Salary adjustment removed");
    } catch (error) {
      store.setNotice(error.message);
    }
  }

  async function generateSalary(year, month) {
    try {
      const generated = store.isDemoMode
        ? businessUnits.map((unit) => {
            const unitDetails = salaryDetails.filter((salary) => Number(salary.intBusinessUnitId) === Number(unit.intBusinessUnitId));
            const unitAdjustments = salaryAdjustments.filter((adjustment) => Number(adjustment.intBusinessUnitId) === Number(unit.intBusinessUnitId) && Number(adjustment.intYear) === Number(year) && Number(adjustment.intMonth) === Number(month));
            const additions = unitAdjustments.filter((adjustment) => adjustment.isAddition).reduce((sum, adjustment) => sum + Number(adjustment.numAmount || 0), 0);
            const deductions = unitAdjustments.filter((adjustment) => adjustment.isDeduction).reduce((sum, adjustment) => sum + Number(adjustment.numAmount || 0), 0);
            return {
              intBusinessUnitId: unit.intBusinessUnitId,
              strBusinessUnitName: unit.strBusinessUnitName,
              intTotalEmployee: unitDetails.length,
              numTotalPaybleSalary: unitDetails.reduce((sum, salary) => sum + Number(salary.numGrossSalary || 0), 0) + additions - deductions
            };
          })
        : await api(`/Salary/SalaryGenerate?intYearId=${year}&intMonthId=${month}`, { method: "POST" });
      store.localReplace("salaryGeneration", generated);
      store.setNotice("Salary summary generated");
    } catch (error) {
      store.setNotice(error.message);
    }
  }

  function removeGeneratedSalary(index) {
    store.localReplace("salaryGeneration", salaryGeneration.filter((_, rowIndex) => rowIndex !== index));
    store.setNotice("Generated salary removed from this run");
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
            <button className="icon-button" onClick={() => logout()} type="button" title="Sign out">
              <LogOut size={18} />
            </button>
          </div>
        </header>

        {active === "dashboard" && (
          <Dashboard
            activeCount={activeCount}
            totalSalary={totalSalary}
            payrollGroups={payrollGroups}
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
            salaryGeneration={salaryGeneration}
            generateSalary={generateSalary}
            removeGeneratedSalary={removeGeneratedSalary}
          />
        )}

        {active === "salary" && (
          <SalaryView
            businessUnits={businessUnits}
            payrollGroups={payrollGroups}
            payrollGroupRows={payrollGroupRows}
            payrollGroupForm={payrollGroupForm}
            setPayrollGroupForm={setPayrollGroupForm}
            payrollGroupErrors={formErrors.payrollGroup}
            savePayrollGroup={savePayrollGroup}
            editPayrollGroup={editPayrollGroup}
            removePayrollGroup={removePayrollGroup}
            updatePayrollGroupRow={updatePayrollGroupRow}
            addPayrollGroupRow={addPayrollGroupRow}
            removePayrollGroupRow={removePayrollGroupRow}
            salaryDetails={salaryDetails}
            salaryCandidates={salaryCandidates}
            salaryAssignForm={salaryAssignForm}
            setSalaryAssignForm={setSalaryAssignForm}
            salaryAssignErrors={formErrors.salaryAssign}
            saveSalaryAssignment={saveSalaryAssignment}
            adjustmentForm={adjustmentForm}
            setAdjustmentForm={setAdjustmentForm}
            adjustmentErrors={formErrors.adjustment}
            saveAdjustment={saveAdjustment}
            totalSalary={totalSalary}
            employees={employees}
            salaryAdjustments={salaryAdjustments}
            removeSalaryAssignment={removeSalaryAssignment}
            removeAdjustment={removeAdjustment}
            businessUnitSummary={businessUnitSummary}
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

function Dashboard({ activeCount, totalSalary, payrollGroups, employees, setActive }) {
  const recent = employees.slice(0, 4);
  return (
    <section className="screen-grid dashboard-grid">
      <Metric icon={UsersRound} label="Active employees" value={activeCount} tone="green" />
      <Metric icon={CircleDollarSign} label="Assigned salary" value={`$${totalSalary.toLocaleString()}`} tone="blue" />
      <Metric icon={ClipboardList} label="Salary structures" value={payrollGroups.length} tone="amber" />

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
            ["Salary", "Structures, assignments, adjustments", Banknote, "salary"],
            ["Payroll", "Generate monthly payable summary", WalletCards, "payroll"]
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

function SalaryView(props) {
  const [salaryTab, setSalaryTab] = useState("structure");
  const [structureSearch, setStructureSearch] = useState("");
  const [salarySearch, setSalarySearch] = useState("");
  const [adjustmentSearch, setAdjustmentSearch] = useState("");
  const totalPercent = props.payrollGroupForm.payrollGroupRowList.reduce((sum, row) => sum + Number(row.numNumberOfPercent || 0), 0);
  const assignableEmployees = [
    ...props.salaryCandidates,
    ...props.salaryDetails.map((salary) => ({
      intEmployeeId: salary.intEmployeeId,
      intBusinessUnitId: salary.intBusinessUnitId,
      strEmployeeName: salary.strEmployeeName,
      strBusinessUnitName: salary.strBusinessUnitName,
      strDepartmentName: salary.strDepartmentName,
      strDesignationName: salary.strDesignationName
    }))
  ];
  const assignedEmployeeIds = new Set(props.salaryDetails.map((salary) => Number(salary.intEmployeeId)));
  const adjustmentEmployees = props.employees.filter((employee) => assignedEmployeeIds.has(Number(employee.intEmployeeBasicInfoId)));
  const salaryTerm = salarySearch.trim().toLowerCase();
  const structureTerm = structureSearch.trim().toLowerCase();
  const adjustmentTerm = adjustmentSearch.trim().toLowerCase();
  const structureRows = structureTerm
    ? props.payrollGroups.filter((group) => [
        group.strPayrollGroupHeaderTitle,
        group.strBusinessUnitName,
        (props.payrollGroupRows[group.intPayrollGroupHeaderId] || []).map((row) => `${row.strPayrollElementName} ${row.numNumberOfPercent}%`).join(" ")
      ].join(" ").toLowerCase().includes(structureTerm))
    : props.payrollGroups;
  const salaryRows = salaryTerm
    ? props.salaryDetails.filter((salary) => [salary.strEmployeeName, salary.strBusinessUnitName, salary.strDepartmentName, salary.strDesignationName, salary.strPayrollGroupHeader].join(" ").toLowerCase().includes(salaryTerm))
    : props.salaryDetails;
  const adjustmentRows = adjustmentTerm
    ? props.salaryAdjustments.filter((adjustment) => [adjustment.strEmployeeName, adjustment.strBusinessUnitName, adjustment.intYear, adjustment.intMonth, adjustmentTypes.find(([id]) => Number(id) === Number(adjustment.intAdditionNdeductionTypeId))?.[1]].join(" ").toLowerCase().includes(adjustmentTerm))
    : props.salaryAdjustments;

  function selectAssignEmployee(employeeId) {
    const employee = assignableEmployees.find((item) => Number(item.intEmployeeId) === Number(employeeId));
    props.setSalaryAssignForm({ ...props.salaryAssignForm, intEmployeeId: employeeId, intBusinessUnitId: employee?.intBusinessUnitId || "" });
  }

  function selectAdjustmentEmployee(employeeId) {
    if (employeeId === "all") {
      props.setAdjustmentForm({ ...props.adjustmentForm, intEmployeeId: "all" });
      return;
    }
    const employee = adjustmentEmployees.find((item) => Number(item.intEmployeeBasicInfoId) === Number(employeeId));
    props.setAdjustmentForm({ ...props.adjustmentForm, intEmployeeId: employeeId, intBusinessUnitId: employee?.intBusinessUnitId || props.adjustmentForm.intBusinessUnitId });
  }

  function selectAdjustmentBusinessUnit(businessUnitId) {
    if (businessUnitId === "") {
      props.setAdjustmentForm({ ...props.adjustmentForm, intBusinessUnitId: "", intEmployeeId: "" });
      return;
    }
    const selectedEmployee = adjustmentEmployees.find((item) => Number(item.intEmployeeBasicInfoId) === Number(props.adjustmentForm.intEmployeeId));
    const keepEmployee = props.adjustmentForm.intEmployeeId === "" || props.adjustmentForm.intEmployeeId === "all" || businessUnitId === "all" || Number(selectedEmployee?.intBusinessUnitId) === Number(businessUnitId);
    props.setAdjustmentForm({
      ...props.adjustmentForm,
      intBusinessUnitId: businessUnitId,
      intEmployeeId: keepEmployee ? props.adjustmentForm.intEmployeeId : "all"
    });
  }

  const filteredAdjustmentEmployees = props.adjustmentForm.intBusinessUnitId === "all" || props.adjustmentForm.intBusinessUnitId === ""
    ? adjustmentEmployees
    : adjustmentEmployees.filter((employee) => Number(employee.intBusinessUnitId) === Number(props.adjustmentForm.intBusinessUnitId));

  return (
    <section className="screen-grid salary-grid">
      <div className="metrics-row">
        <Metric icon={Banknote} label="Gross salary" value={`$${props.totalSalary.toLocaleString()}`} tone="green" />
        <Metric icon={UsersRound} label="Employees tracked" value={props.employees.length} tone="blue" />
        <Metric icon={Building2} label="Business units" value={props.businessUnitSummary} tone="amber" />
      </div>

      <section className="salary-subnav salary-wide">
        <div className="module-tabs" role="tablist" aria-label="Salary sections">
          {[
            ["structure", "Structure", WalletCards],
            ["assign", "Assign", BadgeDollarSign],
            ["adjustments", "Adjustments", CircleDollarSign]
          ].map(([id, label, Icon]) => (
            <button className={salaryTab === id ? "active" : ""} type="button" key={id} onClick={() => setSalaryTab(id)}>
              <Icon size={17} /> {label}
            </button>
          ))}
        </div>
      </section>

      {salaryTab === "structure" && (
        <section className="salary-segment salary-wide">
          <section className="workspace-band form-panel">
            <form onSubmit={props.savePayrollGroup} noValidate>
              <FormTitle icon={WalletCards} title={props.payrollGroupForm.intPayrollGroupHeaderId ? "Edit salary structure" : "Salary structure"} />
              <ValidationSummary errors={props.payrollGroupErrors} />
              <div className="form-grid">
                <TextField label="Structure title" value={props.payrollGroupForm.strPayrollGroupHeaderTitle} onChange={(value) => props.setPayrollGroupForm({ ...props.payrollGroupForm, strPayrollGroupHeaderTitle: value })} required />
                <SelectField
                  label="Business unit"
                  value={props.payrollGroupForm.intBusinessUnitId}
                  onChange={(value) => props.setPayrollGroupForm({ ...props.payrollGroupForm, intBusinessUnitId: value })}
                  options={props.businessUnits.map((item) => [item.intBusinessUnitId, item.strBusinessUnitName])}
                  required
                />
                <ReadOnlyField label="Component total" value={`${totalPercent}%`} />
              </div>
              <div className="structure-list">
                {props.payrollGroupForm.payrollGroupRowList.map((row, index) => (
                  <div className="structure-row" key={`${index}-${row.intPayrollElementTypeId}`}>
                    <TextField label="Component" value={row.strPayrollElementName} onChange={(value) => props.updatePayrollGroupRow(index, "strPayrollElementName", value)} required />
                    <TextField type="number" label="Percent" value={row.numNumberOfPercent} onChange={(value) => props.updatePayrollGroupRow(index, "numNumberOfPercent", value)} required />
                    <button className="icon-button danger" type="button" onClick={() => props.removePayrollGroupRow(index)} title="Remove component">
                      <Trash2 size={16} />
                    </button>
                  </div>
                ))}
              </div>
              <div className="form-actions">
                <button className="ghost-button" type="button" onClick={props.addPayrollGroupRow}><Plus size={17} /> Add component</button>
                <button className="ghost-button" type="button" onClick={() => props.setPayrollGroupForm(emptyPayrollGroup)}><X size={17} /> Clear</button>
                <button className="primary-button" type="submit"><Check size={17} /> Save structure</button>
              </div>
            </form>
          </section>

          <section className="workspace-band table-panel">
            <div className="section-heading">
              <div>
                <p className="eyebrow">Salary structures</p>
                <h2>Component groups</h2>
              </div>
              <label className="search-box">
                <Search size={17} />
                <input value={structureSearch} onChange={(event) => setStructureSearch(event.target.value)} placeholder="Search structures" />
              </label>
            </div>
            <DataTable
              columns={["Structure", "Business unit", "Components", "Actions"]}
              rows={structureRows.map((group) => [
                group.strPayrollGroupHeaderTitle,
                group.strBusinessUnitName,
                (props.payrollGroupRows[group.intPayrollGroupHeaderId] || []).map((row) => `${row.strPayrollElementName} ${row.numNumberOfPercent}%`).join(", "),
                <ActionSet key={group.intPayrollGroupHeaderId} onEdit={() => props.editPayrollGroup(group)} onDelete={() => props.removePayrollGroup(group.intPayrollGroupHeaderId)} />
              ])}
            />
          </section>
        </section>
      )}

      {salaryTab === "assign" && (
        <section className="salary-segment salary-wide">
          <section className="workspace-band form-panel">
            <form onSubmit={props.saveSalaryAssignment} noValidate>
              <FormTitle icon={BadgeDollarSign} title={props.salaryAssignForm.intSalaryAssignHeaderId ? "Edit salary" : "Assign salary"} />
              <ValidationSummary errors={props.salaryAssignErrors} />
              <div className="form-grid">
                <SelectField label="Employee" value={props.salaryAssignForm.intEmployeeId} onChange={selectAssignEmployee} options={assignableEmployees.map((item) => [item.intEmployeeId, item.strEmployeeName])} required />
                <SelectField label="Business unit" value={props.salaryAssignForm.intBusinessUnitId} onChange={(value) => props.setSalaryAssignForm({ ...props.salaryAssignForm, intBusinessUnitId: value, intPayrollGroupHeaderId: "" })} options={props.businessUnits.map((item) => [item.intBusinessUnitId, item.strBusinessUnitName])} required />
                <SelectField label="Salary structure" value={props.salaryAssignForm.intPayrollGroupHeaderId} onChange={(value) => props.setSalaryAssignForm({ ...props.salaryAssignForm, intPayrollGroupHeaderId: value })} options={props.payrollGroups.filter((group) => Number(group.intBusinessUnitId) === Number(props.salaryAssignForm.intBusinessUnitId)).map((group) => [group.intPayrollGroupHeaderId, group.strPayrollGroupHeaderTitle])} required />
                <TextField type="number" label="Gross salary" value={props.salaryAssignForm.numGrossSalary} onChange={(value) => props.setSalaryAssignForm({ ...props.salaryAssignForm, numGrossSalary: value, numNetGrossSalary: value })} required />
              </div>
              <div className="form-actions">
                <button className="primary-button" type="submit"><Plus size={17} /> Save salary</button>
                <button className="ghost-button" type="button" onClick={() => props.setSalaryAssignForm(emptySalaryAssign)}><X size={17} /> Clear</button>
              </div>
            </form>
          </section>

          <section className="workspace-band table-panel">
            <div className="section-heading">
              <div>
                <p className="eyebrow">Salary desk</p>
                <h2>Assigned salary details</h2>
              </div>
              <label className="search-box">
                <Search size={17} />
                <input value={salarySearch} onChange={(event) => setSalarySearch(event.target.value)} placeholder="Search assigned salary" />
              </label>
            </div>
            <DataTable
              columns={["Employee", "Department", "Designation", "Structure", "Gross salary", "Actions"]}
              rows={salaryRows.map((salary) => [
                salary.strEmployeeName,
                salary.strDepartmentName,
                salary.strDesignationName,
                salary.strPayrollGroupHeader,
                `$${Number(salary.numGrossSalary || 0).toLocaleString()}`,
                <ActionSet
                  key={salary.intEmployeeId}
                  onEdit={() => props.setSalaryAssignForm({
                    ...emptySalaryAssign,
                    intSalaryAssignHeaderId: salary.intSalaryAssignHeaderId || 0,
                    intEmployeeId: salary.intEmployeeId,
                    intBusinessUnitId: salary.intBusinessUnitId,
                    intPayrollGroupHeaderId: salary.intPayrollGroupHeader,
                    numGrossSalary: salary.numGrossSalary,
                    numNetGrossSalary: salary.numNetGrossSalary || salary.numGrossSalary
                  })}
                  onDelete={() => props.removeSalaryAssignment(salary.intSalaryAssignHeaderId)}
                />
              ])}
            />
          </section>
        </section>
      )}

      {salaryTab === "adjustments" && (
        <section className="salary-segment salary-wide">
          <section className="workspace-band form-panel">
            <form onSubmit={props.saveAdjustment} noValidate>
              <FormTitle icon={CircleDollarSign} title={props.adjustmentForm.intSalaryAdditionAndDeductionId ? "Edit adjustment" : "Monthly adjustment"} />
              <ValidationSummary errors={props.adjustmentErrors} />
              <div className="movement-choice compact">
                <label className={props.adjustmentForm.isAddition ? "choice-card active" : "choice-card"}>
                  <input type="radio" checked={props.adjustmentForm.isAddition} onChange={() => props.setAdjustmentForm({ ...props.adjustmentForm, isAddition: true, isDeduction: false })} />
                  <Plus size={18} />
                  <span><strong>Addition</strong><small>Allowance, overtime or one-time payment.</small></span>
                </label>
                <label className={props.adjustmentForm.isDeduction ? "choice-card active" : "choice-card"}>
                  <input type="radio" checked={props.adjustmentForm.isDeduction} onChange={() => props.setAdjustmentForm({ ...props.adjustmentForm, isAddition: false, isDeduction: true })} />
                  <Trash2 size={18} />
                  <span><strong>Deduction</strong><small>Tax, advance or penalty.</small></span>
                </label>
              </div>
              <div className="form-grid">
                <SelectField label="Business unit" value={props.adjustmentForm.intBusinessUnitId} onChange={selectAdjustmentBusinessUnit} options={[["all", "All Business Units"], ...props.businessUnits.map((item) => [item.intBusinessUnitId, item.strBusinessUnitName])]} required />
                <SelectField label="Employee" value={props.adjustmentForm.intEmployeeId} onChange={selectAdjustmentEmployee} options={[["all", "All Employees"], ...filteredAdjustmentEmployees.map((item) => [item.intEmployeeBasicInfoId, item.strEmployeeName])]} required />
                <SelectField label="Type" value={props.adjustmentForm.intAdditionNdeductionTypeId} onChange={(value) => props.setAdjustmentForm({ ...props.adjustmentForm, intAdditionNdeductionTypeId: value })} options={adjustmentTypes} required />
                <TextField type="number" label="Amount" value={props.adjustmentForm.numAmount} onChange={(value) => props.setAdjustmentForm({ ...props.adjustmentForm, numAmount: value })} required />
                <TextField type="number" label="Year" value={props.adjustmentForm.intYear} onChange={(value) => props.setAdjustmentForm({ ...props.adjustmentForm, intYear: value })} required />
                <SelectField label="Month" value={props.adjustmentForm.intMonth} onChange={(value) => props.setAdjustmentForm({ ...props.adjustmentForm, intMonth: value })} options={months} required />
              </div>
              <div className="form-actions">
                <button className="primary-button" type="submit"><Check size={17} /> Save adjustment</button>
                <button className="ghost-button" type="button" onClick={() => props.setAdjustmentForm(emptyAdjustment)}><X size={17} /> Clear</button>
              </div>
            </form>
          </section>

          <section className="workspace-band table-panel">
            <div className="section-heading">
              <div>
                <p className="eyebrow">Adjustments</p>
                <h2>Additions and deductions</h2>
              </div>
              <label className="search-box">
                <Search size={17} />
                <input value={adjustmentSearch} onChange={(event) => setAdjustmentSearch(event.target.value)} placeholder="Search adjustments" />
              </label>
            </div>
            <DataTable
              columns={["Employee", "Business unit", "Period", "Type", "Direction", "Amount", "Actions"]}
              rows={adjustmentRows.map((row) => [
                row.strEmployeeName,
                row.strBusinessUnitName,
                `${months.find(([id]) => Number(id) === Number(row.intMonth))?.[1] || row.intMonth} ${row.intYear || ""}`,
                adjustmentTypes.find(([id]) => Number(id) === Number(row.intAdditionNdeductionTypeId))?.[1] || "Adjustment",
                row.isAddition ? "Addition" : "Deduction",
                `$${Number(row.numAmount || 0).toLocaleString()}`,
                <ActionSet
                  key={row.intSalaryAdditionAndDeductionId}
                  onEdit={() => props.setAdjustmentForm({
                    ...emptyAdjustment,
                    intSalaryAdditionAndDeductionId: row.intSalaryAdditionAndDeductionId,
                    intBusinessUnitId: row.intBusinessUnitId,
                    intEmployeeId: row.intEmployeeId,
                    intYear: row.intYear,
                    intMonth: row.intMonth,
                    intAdditionNdeductionTypeId: row.intAdditionNdeductionTypeId,
                    numAmount: row.numAmount,
                    isAddition: row.isAddition,
                    isDeduction: row.isDeduction
                  })}
                  onDelete={() => props.removeAdjustment(row.intSalaryAdditionAndDeductionId)}
                />
              ])}
            />
          </section>
        </section>
      )}
    </section>
  );
}

function PayrollView(props) {
  const [generatePeriod, setGeneratePeriod] = useState({ year: new Date().getFullYear(), month: new Date().getMonth() + 1 });

  return (
    <section className="screen-grid payroll-grid">
      <section className="workspace-band table-panel salary-wide">
        <div className="section-heading">
          <div>
            <p className="eyebrow">Payroll run</p>
            <h2>Monthly payable summary</h2>
          </div>
          <div className="period-controls">
            <TextField type="number" label="Year" value={generatePeriod.year} onChange={(value) => setGeneratePeriod({ ...generatePeriod, year: value })} />
            <SelectField label="Month" value={generatePeriod.month} onChange={(value) => setGeneratePeriod({ ...generatePeriod, month: value })} options={months} />
            <button className="primary-button" type="button" onClick={() => props.generateSalary(generatePeriod.year, generatePeriod.month)}><Banknote size={17} /> Generate</button>
          </div>
        </div>
        <DataTable
          columns={["Business unit", "Employees", "Payable salary", "Actions"]}
          rows={props.salaryGeneration.map((row, index) => [
            row.strBusinessUnitName,
            row.intTotalEmployee,
            `$${Number(row.numTotalPaybleSalary || row.numTotalPayableSalary || 0).toLocaleString()}`,
            <ActionSet key={`${row.intBusinessUnitId}-${index}`} onDelete={() => props.removeGeneratedSalary(index)} />
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
      {onEdit && <button className="icon-button" type="button" title="Edit" onClick={onEdit}><Edit3 size={16} /></button>}
      {onDelete && <button className="icon-button danger" type="button" title="Delete" onClick={onDelete}><Trash2 size={16} /></button>}
    </div>
  );
}

createRoot(document.getElementById("root")).render(<App />);
