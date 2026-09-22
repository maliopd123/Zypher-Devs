CREATE DATABASE zypher_dev;
USE zypher_dev;

#CREATE DATABASE IF NOT EXISTS zypher_dev;

USE zypher_dev;

CREATE TABLE IF NOT EXISTS usuarios (
    id_usu INT AUTO_INCREMENT PRIMARY KEY,
    nome_usu VARCHAR(100) NOT NULL,
    email_usu VARCHAR(150) NOT NULL,
    senha_usu VARCHAR(100) NOT NULL
);
INSERT INTO usuarios (nome_usu, email_usu, senha_usu)
VALUES
('Abraao', 'abraao@gmail.com', '123456'),
('Joao', 'joao@gmail.com', '123456'),
('Isaac', 'isaac@gmail.com', '123456'),
('Arthur', 'arthur@gmail.com', '123456')usuariosusuarios,
('Adrian', 'adrian@gmail.com', '123456');
SELECT * FROM usuarios;

