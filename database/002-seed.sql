SET XACT_ABORT ON;
BEGIN TRANSACTION;
IF NOT EXISTS (SELECT 1 FROM dbo.Checks)
INSERT dbo.Checks(Area,Title,Owner,Critical,Complete) VALUES
 (N'Engineering',N'Baseline setup reviewed',N'Race engineer',1,1),
 (N'Garage',N'Wheel torque independently verified',N'Chief mechanic',1,0),
 (N'Systems',N'Radio and emergency channel checked',N'Systems engineer',1,1),
 (N'Energy',N'Stint fuel target signed off',N'Strategist',1,0),
 (N'Driver',N'Session objectives briefed',N'Driver coach',0,1),
 (N'Data',N'Post-session analysis workspace prepared',N'Data engineer',0,0);
COMMIT;
