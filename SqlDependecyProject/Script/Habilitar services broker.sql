--con la siguiente linea se ve si esta habilitado el Service Broker para cada base de datos.
SELECT NAME, IS_BROKER_ENABLED FROM SYS.DATABASES

--con la siguiente linea se habilita el service broker
ALTER DATABASE Prototipo1 SET ENABLE_BROKER

--si la anterior linea se colgo es por que hay cosas pendientes, entonces usa la siguiente linea que cierra todo!
alter database Prototipo1 set enable_broker with rollback immediate;