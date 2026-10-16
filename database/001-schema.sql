SET XACT_ABORT ON;
BEGIN TRANSACTION;
IF OBJECT_ID('dbo.Checks') IS NULL
BEGIN
 CREATE TABLE dbo.Checks (
 Id int IDENTITY PRIMARY KEY, Area nvarchar(60) NOT NULL,
 Title nvarchar(160) NOT NULL, Owner nvarchar(60) NOT NULL,
 Critical bit NOT NULL, Complete bit NOT NULL DEFAULT 0,
 Version rowversion NOT NULL);
 CREATE TABLE dbo.Audit (
 Id bigint IDENTITY PRIMARY KEY,
 CheckId int NOT NULL REFERENCES dbo.Checks(Id),
 Complete bit NOT NULL, ChangedAt datetime2 NOT NULL);
 CREATE INDEX IX_Audit_CheckId ON dbo.Audit(CheckId);
END;
COMMIT;
