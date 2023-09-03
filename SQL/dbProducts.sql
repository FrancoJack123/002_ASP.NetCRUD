CREATE DATABASE PROYECT_CRUD_ASP
GO

USE PROYECT_CRUD_ASP
GO

IF OBJECT_ID('tb_producto', 'U') IS NOT NULL
    DROP TABLE tb_producto;

CREATE TABLE tb_producto (
    cod_producto INT PRIMARY KEY IDENTITY(1,1),
    descrip_prod VARCHAR(50) NULL,
    stock INT NULL,
    pventa FLOAT NULL,
    fecha_venc DATE NULL,
    activo BIT NOT NULL
);

INSERT INTO tb_producto (descrip_prod, stock, pventa, fecha_venc, activo)
VALUES
    ('Fideos de trigo', 100, 1.99, '2023-12-31', 1),
    ('Arroz integral', 75, 2.49, '2023-11-15', 1),
    ('Lentejas orgánicas', 50, 3.99, '2023-10-20', 1),
    ('Aceite de oliva extra virgen', 40, 6.99, '2024-03-01', 1);

SELECT * FROM tb_producto;