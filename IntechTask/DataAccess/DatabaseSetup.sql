DROP TABLE phone_type
GO
DROP TABLE gender
GO
DROP TABLE document_type
GO
DROP TABLE document
GO
DROP TABLE employee_phone
GO
DROP TABLE employee
GO

--a. Сотрудники (хранение информации о сотрудниках)
-- ФИО
-- Пол
-- Дата рождения
--Дата рождения должна контролироваться, чтобы возраст был 18 до 100 лет
--Не должно быть совпадений по ФИО + Дата рождения

CREATE TABLE gender
(
	gender_id INT PRIMARY KEY IDENTITY(1, 1),
	name NVARCHAR(100) NOT NULL UNIQUE
)

CREATE TABLE employee
(
	employee_id INT PRIMARY KEY IDENTITY (1, 1),
	gender_id INT NOT NULL,
	fullname NVARCHAR(100) NOT NULL,	
	date_of_birth DATE NOT NULL,
	CONSTRAINT IX_employee_fullname_date_of_birth UNIQUE (fullname, date_of_birth),
	CONSTRAINT date_of_birth CHECK (date_of_birth <= DATEADD(YEAR, -18, GETDATE()) AND date_of_birth >= DATEADD(YEAR, -100, GETDATE())),
	CONSTRAINT FK_employee_gender FOREIGN KEY (gender_id) REFERENCES gender (gender_id) ON DELETE CASCADE
)

--Для остальных таблиц и представлений не нужно делать интерфейс в программе (только скрипт)
--b. Документы (для хранения данных о документах сотрудника)
-- Серия / Номер
-- Тип документа (отдельный справочник)
-- Дата выдачи
-- Кем выдан
--Не должно быть совпадений по Серия / Номер + Тип документа

CREATE TABLE document_type
(
	document_type_id INT PRIMARY KEY IDENTITY (1, 1),
	name NVARCHAR(500) NOT NULL
)

CREATE TABLE document
(
	document_id INT PRIMARY KEY IDENTITY (1, 1),
	document_type_id INT NOT NULL,
	issued_by INT NOT NULL,
	serial_number NVARCHAR(100) NOT NULL,
	issued_at DATE NOT NULL,
	CONSTRAINT IX_document_serial_number_document_type_id UNIQUE (serial_number, document_type_id),
	CONSTRAINT FK_document_document_type FOREIGN KEY (document_type_id) REFERENCES document_type (document_type_id) ON DELETE CASCADE,
	CONSTRAINT FK_document_employee FOREIGN KEY (issued_by) REFERENCES employee (employee_id) ON DELETE CASCADE
)

--c. Телефоны (для хранения телефонов сотрудников)
-- Номер
-- Тип телефона (отдельный справочник)
--Не должно быть совпадений по номеру телефона

CREATE TABLE phone_type
(
	phone_type_id INT PRIMARY KEY IDENTITY (1, 1),
	name NVARCHAR(100) NOT NULL
)

CREATE TABLE employee_phone
(
	employee_phone_id INT PRIMARY KEY IDENTITY (1, 1),
	employee_id INT NOT NULL,
	number VARCHAR(20) NOT NULL UNIQUE,
	CONSTRAINT FK_employee_phone_employee FOREIGN KEY (employee_id) REFERENCES employee (employee_id) ON DELETE CASCADE
)

GO

INSERT INTO gender (name) VALUES ('male'), ('female')

INSERT INTO employee (gender_id, fullname, date_of_birth) VALUES (1, 'john doe', '2000-01-10'), (2, 'jane doe', '1995-02-20')