INSERT INTO "Accounts"
("Id", "Phone", "PasswordHash", "Name", "Role", "RoleDataJson", "CreationTime", "IsActive", "Comment")
VALUES
('db42b4ec-772e-46a9-93c2-246431d9292b', '3537434871', 'ee2580265a042f4220ff9312a41833066b23532fdf4dbf5a752a3e739a4c5ce0', 'Егоров Егор Егорович', 'Employee', '{"segmentId": 1, "gender": "М", "birthDate": "2000-01-10", "employmentDate": "2019-09-10"}', '2024-03-20', true, 'Самый главный в цехе'),
('65db6d00-70f8-4947-ae48-92c874423932', '2222222222', 'ee2580265a042f4220ff9312a41833066b23532fdf4dbf5a752a3e739a4c5ce0', 'Жаннова Жанна Жановна', 'Employee', '{"segmentId": 2, "gender": "Ж", "birthDate": "1990-03-30", "employmentDate": "2010-08-11"}', '2024-03-10',  true, NULL),
('151d781d-a8a3-42c1-bb35-e479841880dd', '9858739578', 'ee2580265a042f4220ff9312a41833066b23532fdf4dbf5a752a3e739a4c5ce0', 'Григорьев Григорий Григорьевич', 'Employee', '{"segmentId": 3, "gender": "М", "birthDate": "1975-01-20", "employmentDate": "2009-03-15"}', '2024-02-20', false, NULL);
