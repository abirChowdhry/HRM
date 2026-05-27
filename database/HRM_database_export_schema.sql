IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240210155126_empbasicCreated'
)
BEGIN
    CREATE TABLE [empBasicInfos] (
        [IntEmployeeBasicInfoId] bigint NOT NULL IDENTITY,
        [StrEmployeeCode] nvarchar(max) NOT NULL,
        [StrEmployeeName] nvarchar(max) NOT NULL,
        [IntDepartmentId] bigint NOT NULL,
        [IntDesignationId] bigint NOT NULL,
        [StrGender] nvarchar(max) NULL,
        [StrReligion] nvarchar(max) NULL,
        [StrMaritalStatus] nvarchar(max) NULL,
        [StrBloodGroup] nvarchar(max) NULL,
        [DteDateOfBirth] datetime2 NULL,
        [DteJoiningDate] datetime2 NOT NULL,
        [DteInternCloseDate] datetime2 NULL,
        [DteProbationaryCloseDate] datetime2 NULL,
        [DteConfirmationDate] datetime2 NULL,
        [DteLastWorkingDate] datetime2 NULL,
        [IntSupervisorId] bigint NULL,
        [IntLineManagerId] bigint NULL,
        [IsActive] bit NULL,
        [IntBusinessUnitId] bigint NOT NULL,
        [IntEmploymentTypeId] bigint NOT NULL,
        [DteCreatedAt] datetime2 NOT NULL,
        [IntCreatedBy] bigint NULL,
        [DteUpdatedAt] datetime2 NULL,
        [IntUpdatedBy] bigint NULL,
        CONSTRAINT [PK_empBasicInfos] PRIMARY KEY ([IntEmployeeBasicInfoId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240210155126_empbasicCreated'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240210155126_empbasicCreated', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240210172502_businessunitDepartmentDesignationEmployementType_Created'
)
BEGIN
    CREATE TABLE [businessUnits] (
        [IntBusinessUnitId] bigint NOT NULL IDENTITY,
        [StrBusinessUnitName] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_businessUnits] PRIMARY KEY ([IntBusinessUnitId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240210172502_businessunitDepartmentDesignationEmployementType_Created'
)
BEGIN
    CREATE TABLE [departments] (
        [IntDepartmentId] bigint NOT NULL IDENTITY,
        [StrDepartmentName] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_departments] PRIMARY KEY ([IntDepartmentId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240210172502_businessunitDepartmentDesignationEmployementType_Created'
)
BEGIN
    CREATE TABLE [designations] (
        [IntDesignationId] bigint NOT NULL IDENTITY,
        [StrDesignationName] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_designations] PRIMARY KEY ([IntDesignationId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240210172502_businessunitDepartmentDesignationEmployementType_Created'
)
BEGIN
    CREATE TABLE [employementTypes] (
        [IntEmployementId] bigint NOT NULL IDENTITY,
        [StrEmployementName] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_employementTypes] PRIMARY KEY ([IntEmployementId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240210172502_businessunitDepartmentDesignationEmployementType_Created'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240210172502_businessunitDepartmentDesignationEmployementType_Created', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240211045000_payrollElementPolicyCreated'
)
BEGIN
    CREATE TABLE [payrollElements] (
        [IntPayrollElementId] bigint NOT NULL IDENTITY,
        [StrPayrollElementName] nvarchar(max) NULL,
        [IntBusinessUnitId] bigint NOT NULL,
        [IsBasicElement] bit NULL,
        [IsSalaryElement] bit NULL,
        [IsActive] bit NULL,
        [DteCreatedAt] datetime2 NOT NULL,
        [IntCreatedBy] bigint NULL,
        [DteUpdatedAt] datetime2 NULL,
        [IntUpdatedBy] bigint NULL,
        CONSTRAINT [PK_payrollElements] PRIMARY KEY ([IntPayrollElementId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240211045000_payrollElementPolicyCreated'
)
BEGIN
    CREATE TABLE [payrollPolicies] (
        [IntPayrollPolicyId] bigint NOT NULL IDENTITY,
        [IntBusinessUnitId] bigint NOT NULL,
        [StrPayrollPolicyName] nvarchar(max) NOT NULL,
        [IsSalaryDivideByActualMonthDays] bit NULL,
        [IntGrossSalaryDevidedByDays] bigint NULL,
        [IntGrossSalaryRoundDigits] bigint NULL,
        [IsGrossSalaryRoundUp] bit NULL,
        [IsGrossSalaryRoundDown] bit NULL,
        [IntNetPayableSalaryRoundDigits] bigint NULL,
        [IsNetPayableSalaryRoundUp] bit NULL,
        [IsNetPayableSalaryRoundDown] bit NULL,
        [IsActive] bit NOT NULL,
        [DteCreatedAt] datetime2 NULL,
        [IntCreatedBy] bigint NULL,
        [DteUpdatedAt] datetime2 NULL,
        [IntUpdatedBy] bigint NULL,
        CONSTRAINT [PK_payrollPolicies] PRIMARY KEY ([IntPayrollPolicyId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240211045000_payrollElementPolicyCreated'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240211045000_payrollElementPolicyCreated', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240211074806_payrollGroupHearderAndRowAdded'
)
BEGIN
    CREATE TABLE [payrollGroupHeaders] (
        [IntPayrollGroupHeaderId] bigint NOT NULL IDENTITY,
        [StrPayrollGroupHeaderTitle] nvarchar(max) NOT NULL,
        [IntPayrollPolicyId] bigint NULL,
        [IsActive] bit NOT NULL,
        [DteCreatedAt] datetime2 NOT NULL,
        [IntCreatedBy] bigint NULL,
        [DteUpdatedAt] datetime2 NULL,
        [IntUpdatedBy] bigint NULL,
        CONSTRAINT [PK_payrollGroupHeaders] PRIMARY KEY ([IntPayrollGroupHeaderId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240211074806_payrollGroupHearderAndRowAdded'
)
BEGIN
    CREATE TABLE [payrollGroupRows] (
        [IntPayrollGroupRowId] bigint NOT NULL IDENTITY,
        [IntPayrollHeaderId] bigint NOT NULL,
        [IntPayrollElementTypeId] bigint NOT NULL,
        [StrPayrollElementName] nvarchar(max) NOT NULL,
        [NumNumberOfPercent] decimal(18,2) NULL,
        [IsActive] bit NOT NULL,
        [DteCreatedAt] datetime2 NOT NULL,
        [IntCreatedBy] bigint NOT NULL,
        [DteUpdatedAt] datetime2 NULL,
        [IntUpdatedBy] bigint NULL,
        CONSTRAINT [PK_payrollGroupRows] PRIMARY KEY ([IntPayrollGroupRowId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240211074806_payrollGroupHearderAndRowAdded'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240211074806_payrollGroupHearderAndRowAdded', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240211081006_payrollGroupHeaderModified'
)
BEGIN
    ALTER TABLE [payrollGroupHeaders] ADD [IntBusinessUnitId] bigint NOT NULL DEFAULT CAST(0 AS bigint);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240211081006_payrollGroupHeaderModified'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240211081006_payrollGroupHeaderModified', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240211141030_salaryAssignHeaderNRowCreated'
)
BEGIN
    CREATE TABLE [salaryAssignHeaders] (
        [IntSalaryAssignHeaderId] bigint NOT NULL IDENTITY,
        [IntPayrollGroupHeaderId] bigint NULL,
        [StrPayrollGroupHeaderTitle] nvarchar(max) NOT NULL,
        [IntEmployeeId] bigint NOT NULL,
        [IntBusinessUnitId] bigint NOT NULL,
        [NumGrossSalary] decimal(18,2) NOT NULL,
        [NumNetGrossSalary] decimal(18,2) NOT NULL,
        [IsActive] bit NULL,
        [IntCreateBy] bigint NULL,
        [DteCreateDateTime] datetime2 NULL,
        [IntUpdateBy] bigint NULL,
        [DteUpdateDateTime] datetime2 NULL,
        CONSTRAINT [PK_salaryAssignHeaders] PRIMARY KEY ([IntSalaryAssignHeaderId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240211141030_salaryAssignHeaderNRowCreated'
)
BEGIN
    CREATE TABLE [salaryAssignRows] (
        [IntSalaryAssignRowId] bigint NOT NULL IDENTITY,
        [IntSalaryAssignHeaderId] bigint NOT NULL,
        [IntEmployeeId] bigint NOT NULL,
        [IntPayrollElementId] bigint NOT NULL,
        [StrPayrollElement] nvarchar(max) NOT NULL,
        [NumNumberOfPercent] decimal(18,2) NULL,
        [NumAmount] decimal(18,2) NOT NULL,
        [IsActive] bit NULL,
        [IntCreateBy] bigint NOT NULL,
        [DteCreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_salaryAssignRows] PRIMARY KEY ([IntSalaryAssignRowId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240211141030_salaryAssignHeaderNRowCreated'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240211141030_salaryAssignHeaderNRowCreated', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240211162319_salaryAdditionNDeduction'
)
BEGIN
    CREATE TABLE [salaryAdditionNDeductions] (
        [IntSalaryAdditionAndDeductionId] bigint NOT NULL IDENTITY,
        [IntBusinessUnitId] bigint NOT NULL,
        [IntEmployeeId] bigint NOT NULL,
        [IntYear] bigint NULL,
        [IntMonth] bigint NULL,
        [IsAddition] bit NULL,
        [IsDeduction] bit NULL,
        [IntAdditionNdeductionTypeId] bigint NULL,
        [NumAmount] decimal(18,2) NULL,
        [IsActive] bit NULL,
        [StrCreatedBy] nvarchar(max) NOT NULL,
        [DteCreatedAt] datetime2 NULL,
        [IntUpdatedBy] bigint NULL,
        [DteUpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_salaryAdditionNDeductions] PRIMARY KEY ([IntSalaryAdditionAndDeductionId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240211162319_salaryAdditionNDeduction'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240211162319_salaryAdditionNDeduction', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240211165427_salaryAdditionNDeductionModified'
)
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[salaryAdditionNDeductions]') AND [c].[name] = N'StrCreatedBy');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [salaryAdditionNDeductions] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [salaryAdditionNDeductions] DROP COLUMN [StrCreatedBy];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240211165427_salaryAdditionNDeductionModified'
)
BEGIN
    ALTER TABLE [salaryAdditionNDeductions] ADD [IntCreatedBy] bigint NOT NULL DEFAULT CAST(0 AS bigint);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240211165427_salaryAdditionNDeductionModified'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240211165427_salaryAdditionNDeductionModified', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212093300_salaryGenerateCreated'
)
BEGIN
    CREATE TABLE [salaryGenerates] (
        [IntSalaryGenerateId] bigint NOT NULL IDENTITY,
        [IntBusinessUnitId] bigint NOT NULL,
        [IntYearId] bigint NOT NULL,
        [IntMonthId] bigint NOT NULL,
        [NumTotalAmount] decimal(18,2) NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_salaryGenerates] PRIMARY KEY ([IntSalaryGenerateId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212093300_salaryGenerateCreated'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240212093300_salaryGenerateCreated', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212093503_salaryGenerateTableModified'
)
BEGIN
    ALTER TABLE [salaryGenerates] ADD [DteCreatedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212093503_salaryGenerateTableModified'
)
BEGIN
    ALTER TABLE [salaryGenerates] ADD [IntCreatedBy] bigint NOT NULL DEFAULT CAST(0 AS bigint);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212093503_salaryGenerateTableModified'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240212093503_salaryGenerateTableModified', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212113323_empBasicInfoUpdated'
)
BEGIN
    ALTER TABLE [empBasicInfos] ADD [IsSalaryHold] bit NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212113323_empBasicInfoUpdated'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240212113323_empBasicInfoUpdated', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212162646_empTransferNPromotionCreated'
)
BEGIN
    DROP TABLE [salaryGenerates];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212162646_empTransferNPromotionCreated'
)
BEGIN
    CREATE TABLE [empTransferNPromotions] (
        [IntEmpTransferNPromotionId] bigint NOT NULL IDENTITY,
        [IntEmployeeId] bigint NOT NULL,
        [IntOldBusinessUnitId] bigint NOT NULL,
        [IntNewBusinessUnitId] bigint NOT NULL,
        [IntOldDepartmentId] bigint NOT NULL,
        [IntNewDepartmenId] bigint NOT NULL,
        [IntOldDesignationId] bigint NOT NULL,
        [IntNewDepartmentId] bigint NOT NULL,
        [IsTransfer] bit NULL,
        [IsPromotion] bit NULL,
        [IntCreatedBy] bigint NOT NULL,
        [DteCreatedAt] datetime2 NOT NULL,
        [IntUpdatedBy] bigint NULL,
        [DteUpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_empTransferNPromotions] PRIMARY KEY ([IntEmpTransferNPromotionId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212162646_empTransferNPromotionCreated'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240212162646_empTransferNPromotionCreated', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212164746_empTransferNPromotionUpdated'
)
BEGIN
    DECLARE @var1 sysname;
    SELECT @var1 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[empTransferNPromotions]') AND [c].[name] = N'IntOldDesignationId');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [empTransferNPromotions] DROP CONSTRAINT [' + @var1 + '];');
    ALTER TABLE [empTransferNPromotions] ALTER COLUMN [IntOldDesignationId] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212164746_empTransferNPromotionUpdated'
)
BEGIN
    DECLARE @var2 sysname;
    SELECT @var2 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[empTransferNPromotions]') AND [c].[name] = N'IntOldDepartmentId');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [empTransferNPromotions] DROP CONSTRAINT [' + @var2 + '];');
    ALTER TABLE [empTransferNPromotions] ALTER COLUMN [IntOldDepartmentId] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212164746_empTransferNPromotionUpdated'
)
BEGIN
    DECLARE @var3 sysname;
    SELECT @var3 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[empTransferNPromotions]') AND [c].[name] = N'IntOldBusinessUnitId');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [empTransferNPromotions] DROP CONSTRAINT [' + @var3 + '];');
    ALTER TABLE [empTransferNPromotions] ALTER COLUMN [IntOldBusinessUnitId] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212164746_empTransferNPromotionUpdated'
)
BEGIN
    DECLARE @var4 sysname;
    SELECT @var4 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[empTransferNPromotions]') AND [c].[name] = N'IntNewDepartmentId');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [empTransferNPromotions] DROP CONSTRAINT [' + @var4 + '];');
    ALTER TABLE [empTransferNPromotions] ALTER COLUMN [IntNewDepartmentId] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212164746_empTransferNPromotionUpdated'
)
BEGIN
    DECLARE @var5 sysname;
    SELECT @var5 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[empTransferNPromotions]') AND [c].[name] = N'IntNewDepartmenId');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [empTransferNPromotions] DROP CONSTRAINT [' + @var5 + '];');
    ALTER TABLE [empTransferNPromotions] ALTER COLUMN [IntNewDepartmenId] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212164746_empTransferNPromotionUpdated'
)
BEGIN
    DECLARE @var6 sysname;
    SELECT @var6 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[empTransferNPromotions]') AND [c].[name] = N'IntNewBusinessUnitId');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [empTransferNPromotions] DROP CONSTRAINT [' + @var6 + '];');
    ALTER TABLE [empTransferNPromotions] ALTER COLUMN [IntNewBusinessUnitId] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212164746_empTransferNPromotionUpdated'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240212164746_empTransferNPromotionUpdated', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212182013_empTransferNPromotionUpdated2'
)
BEGIN
    DECLARE @var7 sysname;
    SELECT @var7 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[empTransferNPromotions]') AND [c].[name] = N'IntNewBusinessUnitId');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [empTransferNPromotions] DROP CONSTRAINT [' + @var7 + '];');
    ALTER TABLE [empTransferNPromotions] DROP COLUMN [IntNewBusinessUnitId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212182013_empTransferNPromotionUpdated2'
)
BEGIN
    DECLARE @var8 sysname;
    SELECT @var8 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[empTransferNPromotions]') AND [c].[name] = N'IntNewDepartmentId');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [empTransferNPromotions] DROP CONSTRAINT [' + @var8 + '];');
    ALTER TABLE [empTransferNPromotions] DROP COLUMN [IntNewDepartmentId];
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212182013_empTransferNPromotionUpdated2'
)
BEGIN
    EXEC sp_rename N'[empTransferNPromotions].[IntOldBusinessUnitId]', N'IntNewDesignationId', N'COLUMN';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212182013_empTransferNPromotionUpdated2'
)
BEGIN
    ALTER TABLE [empTransferNPromotions] ADD [IntBusinessUnitId] bigint NOT NULL DEFAULT CAST(0 AS bigint);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212182013_empTransferNPromotionUpdated2'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240212182013_empTransferNPromotionUpdated2', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212211326_bonusSetupCreated'
)
BEGIN
    CREATE TABLE [bonusSetups] (
        [IntBonusSetypId] bigint NOT NULL IDENTITY,
        [IntBusinessUnitId] bigint NOT NULL,
        [IntDepartmentId] bigint NULL,
        [IntServiceLengthMonths] bigint NULL,
        [StrReligion] nvarchar(max) NULL,
        [NumPercentage] decimal(18,2) NOT NULL,
        [IsActive] bit NOT NULL,
        [IntCreatedBy] bigint NOT NULL,
        [DteCreatedAt] datetime2 NOT NULL,
        [IntUpdatedBy] bigint NULL,
        [DteUpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_bonusSetups] PRIMARY KEY ([IntBonusSetypId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212211326_bonusSetupCreated'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240212211326_bonusSetupCreated', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212222102_bonusSetupUpdated'
)
BEGIN
    ALTER TABLE [bonusSetups] ADD [IntEmployementTypeId] bigint NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212222102_bonusSetupUpdated'
)
BEGIN
    ALTER TABLE [bonusSetups] ADD [StrBonusSetupName] nvarchar(max) NOT NULL DEFAULT N'';
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240212222102_bonusSetupUpdated'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240212222102_bonusSetupUpdated', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240213033402_bonusGenerateCreated'
)
BEGIN
    CREATE TABLE [bonusGenerates] (
        [IntBonusGenerateId] bigint NOT NULL IDENTITY,
        [IntBonusSetupId] bigint NOT NULL,
        [IntYearId] bigint NOT NULL,
        [IntMonthId] bigint NOT NULL,
        [IsActive] bit NOT NULL,
        [IntCreatedBy] bigint NOT NULL,
        [DteCreatedAt] datetime2 NOT NULL,
        [IntUpdatedBy] bigint NULL,
        [DteUpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_bonusGenerates] PRIMARY KEY ([IntBonusGenerateId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240213033402_bonusGenerateCreated'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240213033402_bonusGenerateCreated', N'8.0.27');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240214062445_empBasicInfoModified'
)
BEGIN
    ALTER TABLE [empBasicInfos] ADD [StrAddress] nvarchar(max) NULL;
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240214062445_empBasicInfoModified'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240214062445_empBasicInfoModified', N'8.0.27');
END;
GO

COMMIT;
GO

