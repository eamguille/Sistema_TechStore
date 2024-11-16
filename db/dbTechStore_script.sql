CREATE DATABASE dbTechStore;

USE dbTechStore;

CREATE TABLE Usuarios(
	id_usuario VARCHAR(6) NOT NULL PRIMARY KEY,
	nombres VARCHAR(60) NOT NULL,
	apellidos VARCHAR(75) NOT NULL,
	nombre_usuario VARCHAR(50) NOT NULL,
	clave VARCHAR(300) NOT NULL,
	email VARCHAR(100) NOT NULL,
	telefono VARCHAR(24) NOT NULL,
	direccion VARCHAR(128) NOT NULL,
	rol VARCHAR(16) NOT NULL,
	CHECK(rol IN('root','administrador','empleado'))
);

CREATE TABLE Productos(
	id_producto VARCHAR(8) NOT NULL PRIMARY KEY,
	nombre_producto VARCHAR(60) NOT NULL,
	descripcion_producto VARCHAR(512) NOT NULL,
	id_categoria_producto INT NOT NULL,
	id_tipo_producto INT NOT NULL,
	id_modelo INT NOT NULL,
	precio_compra DECIMAL(5,2) NOT NULL,
	precio_venta DECIMAL(5,2) NOT NULL,
	cantidad_stock INT NOT NULL DEFAULT 0,
	estado VARCHAR(24) NOT NULL,
	CHECK(estado IN('Disponible','Agotado'))
);

CREATE TABLE Categorias_Productos(
	id_categoria_producto INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	categoria VARCHAR(50) NOT NULL
);

CREATE TABLE Tipos_Productos(
	id_tipo_producto INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	tipo_producto VARCHAR(60) NOT NULL
);

CREATE TABLE Modelos(
	id_modelo INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	modelo VARCHAR(75) NOT NULL,
	id_marca INT NOT NULL
);

CREATE TABLE Marcas(
	id_marca INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	marca VARCHAR(30) NOT NULL
);

CREATE TABLE Proveedores(
	id_proveedor INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	nombre_proveedor VARCHAR(100) NOT NULL,
	dui VARCHAR(10) NOT NULL UNIQUE,
	telefono VARCHAR(24) NOT NULL UNIQUE,
	email VARCHAR(100) NOT NULL UNIQUE,
	direccion VARCHAR(120) NOT NULL
);

CREATE TABLE Clientes(
	id_cliente INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	nombre_cliente VARCHAR(100) NOT NULL,
	dui VARCHAR(10) NOT NULL UNIQUE,
	telefono VARCHAR(24) NOT NULL UNIQUE,
	email VARCHAR(100) NOT NULL UNIQUE,
	direccion VARCHAR(120) NOT NULL
);

CREATE TABLE Compras_Empresa(
	id_compra INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	id_proveedor INT NOT NULL,
	id_usuario VARCHAR(6) NOT NULL,
	fecha_compra DATE NOT NULL,
	total_compra DECIMAL(7,2) NOT NULL,
	estado_compra VARCHAR(30) NOT NULL,
	CHECK(estado_compra IN('Completada','Pendiente'))
);

CREATE TABLE Ventas_Empresa(
	id_venta INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	id_cliente INT NOT NULL,
	id_usuario VARCHAR(6) NOT NULL,
	fecha_venta DATE NOT NULL,
	total_venta DECIMAL(7,2) NOT NULL,
	metodo_pago VARCHAR(30) NOT NULL,
	estado_venta VARCHAR(30) NOT NULL,
	CHECK(metodo_pago IN('Efectivo','Tarjeta')),
	CHECK(estado_venta IN('Completada','Pendiente'))
);

CREATE TABLE Detalles_Compras(
	id_detalle_compra VARCHAR(7) NOT NULL PRIMARY KEY,
	id_compra INT NOT NULL,
	id_producto VARCHAR(8) NOT NULL,
	cantidad INT NOT NULL DEFAULT 0,
	precio_unitario DECIMAL(6,2)
);

CREATE TABLE Detalles_Ventas(
	id_detalle_venta VARCHAR(7) NOT NULL PRIMARY KEY,
	id_venta INT NOT NULL,
	id_producto VARCHAR(8) NOT NULL,
	cantidad INT NOT NULL DEFAULT 0,
	precio_unitario DECIMAL(6,2)
);

CREATE TABLE Movimientos_Inventario(
	id_movimiento INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	id_producto VARCHAR(8) NOT NULL,
	id_usuario VARCHAR(6) NOT NULL,
	tipo_movimiento VARCHAR(30) NOT NULL,
	cantidad INT NOT NULL,
	fecha_movimiento DATE NOT NULL,
	descripcion_movimiento VARCHAR(224) NOT NULL,
	CHECK(tipo_movimiento IN('entrada', 'salida'))
);


-- AQUI AGREGAMOS LAS RELACIONES
ALTER TABLE Modelos
ADD CONSTRAINT fk_marcaModelo
FOREIGN KEY (id_marca)
REFERENCES Marcas(id_marca);

ALTER TABLE Productos
ADD CONSTRAINT fk_categoriaProducto
FOREIGN KEY (id_categoria_producto)
REFERENCES Categorias_Productos(id_categoria_producto);

ALTER TABLE Productos
ADD CONSTRAINT fk_tipoProducto
FOREIGN KEY (id_tipo_producto)
REFERENCES Tipos_Productos(id_tipo_producto);

ALTER TABLE Productos
ADD CONSTRAINT fk_modeloProducto
FOREIGN KEY (id_modelo)
REFERENCES Modelos(id_modelo);

ALTER TABLE Compras_Empresa
ADD CONSTRAINT fk_compraProveedor
FOREIGN KEY (id_proveedor)
REFERENCES Proveedores(id_proveedor);

ALTER TABLE Compras_Empresa
ADD CONSTRAINT fk_controlUsuarioCompra
FOREIGN KEY (id_usuario)
REFERENCES Usuarios(id_usuario);

ALTER TABLE Ventas_Empresa
ADD CONSTRAINT fk_ventaCliente
FOREIGN KEY (id_cliente)
REFERENCES Clientes(id_cliente);

ALTER TABLE Ventas_Empresa
ADD CONSTRAINT fk_controlUsuarioVenta
FOREIGN KEY (id_usuario)
REFERENCES Usuarios(id_usuario);

ALTER TABLE Detalles_Compras
ADD CONSTRAINT fk_compraDetalle
FOREIGN KEY (id_compra)
REFERENCES Compras_Empresa(id_compra);

ALTER TABLE Detalles_Compras
ADD CONSTRAINT fk_productoCompraDetalle
FOREIGN KEY (id_producto)
REFERENCES Productos(id_producto);

ALTER TABLE Detalles_Ventas
ADD CONSTRAINT fk_ventaDetalle
FOREIGN KEY (id_venta)
REFERENCES Ventas_Empresa(id_venta);

ALTER TABLE Detalles_Ventas
ADD CONSTRAINT fk_productoVentaDetalle
FOREIGN KEY (id_producto)
REFERENCES Productos(id_producto);

ALTER TABLE Movimientos_Inventario
ADD CONSTRAINT fk_productoMovimiento
FOREIGN KEY (id_producto)
REFERENCES Productos(id_producto);

ALTER TABLE Movimientos_Inventario
ADD CONSTRAINT fk_controlUsuarioMovimiento
FOREIGN KEY (id_usuario)
REFERENCES Usuarios(id_usuario);

-- Aqui agregamos insersiones a las tablas genericas
INSERT INTO Categorias_Productos(categoria)
VALUES ('Redes'),
		('Computo'),
		('Electronica'),
		('Gamer'),
		('Oficina');

INSERT INTO Tipos_Productos(tipo_producto)
VALUES ('Celular'),
		('PC'),
		('Laptop'),
		('Componentes'),
		('Monitor');


INSERT INTO Marcas(marca)
VALUES ('MSI'),
		('ASUS'),
		('NVIDIA'),
		('AMD'),
		('CORSAIR'),
		('INTEL'),
		('SAMSUNG'),
		('HP'),
		('APPLE'),
		('HUAWEI'),
		('LG'),
		('DELL'),
		('LENOVO');

INSERT INTO Modelos(modelo, id_marca)
VALUES ('GT', 1),
		('GS', 1),
		('GE', 1),
		('GP/GL', 2),
		('Spice Stellar 361 (Mi-361)', 2),
		('CUPE', 2),
		('SP600', 3),
		('WMA8701A', 3),
		('Micromax X250', 3),
		('HTC Desire Eye', 4),
		('WX295', 4),
		('Ascend Y', 4),
		('Pop Icon', 5),
		('IdeaPad A1',5 ),
		('Kyocera E4600', 5),
		('E1182', 6),
		('Nexus 6P', 6),
		('C320 InTouch Lady', 6),
		('6760 slide', 7),
		('Z800', 7),
		('OT 156', 7),
		('Coolpad Roar', 8),
		('Celkon C333', 8),
		('Bird D680', 8);

-- Aqui creamos un usuario para probar el login en el sistema
INSERT INTO Usuarios
VALUES ('U00001', 'Guillermo', 'Castillo', 'root', '8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92', 'random@gmail.com', '7633-5320', 'San Salvador', 'root')

SELECT * FROM "Ventas_Empresa"

DELETE FROM Usuarios WHERE id_usuario = 'U00003';

INSERT INTO Ventas_Empresa
VALUES (1,'U00003','2024-12-12',700, 'Efectivo', 'Completada');

UPDATE "Ventas_Empresa" SET fecha_venta = '2024-10-12' WHERE id_venta = 3

SELECT * FROM "Productos";

UPDATE Productos SET id_producto = 'PROD00001' WHERE id_producto = 'P00002';
UPDATE Productos SET id_producto = 'PROD00002' WHERE id_producto = 'P00003';
UPDATE Productos SET id_producto = 'PROD00003' WHERE id_producto = 'PROD0000';

INSERT INTO Detalles_Ventas
VALUES ('DV00003',2,'P00002',10,24);