-- Find your user's ID and the ITAgent role's ID
SELECT Id, Email FROM AspNetUsers;
SELECT Id, Name FROM AspNetRoles;

INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES ('73c9b22f-5d2d-480f-866c-c3761494dbab', '49f5da70-4337-40e4-9706-eb22cafcf0e8');

INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES ('73c9b22f-5d2d-480f-866c-c3761494dbab', '29c65eb7-9d8e-48cf-94ce-1d3f3e1b8e31');