DROP TABLE IF EXISTS csvtest1;
CREATE TABLE csvtest1
(
  intfld INT PRIMARY KEY,
  strfld VARCHAR(20),
  doublefld DOUBLE,
  decimalfld DECIMAL(11,2)
);

INSERT INTO csvtest1(intfld, strfld, doublefld, decimalfld)
VALUES (1, 'aaa', 100.10, 1000.11);
INSERT INTO csvtest1(intfld, strfld, doublefld, decimalfld)
VALUES (2, 'bbb', 200.20, 2000.22);
INSERT INTO csvtest1(intfld, strfld, doublefld, decimalfld)
VALUES (3, 'ccc', 300.30, 3000.33);
INSERT INTO csvtest1(intfld, strfld, doublefld, decimalfld)
VALUES (4, 'ddd', 400.40, 4000.44);

DROP TABLE IF EXISTS csvtest2;
CREATE TABLE csvtest2
(
  intr2 INT PRIMARY KEY,
  strr2 VARCHAR(20)
);

INSERT INTO csvtest2 (intr2, strr2)
VALUES (1, "r2_1");
INSERT INTO csvtest2 (intr2, strr2)
VALUES (2, "r2_2");
INSERT INTO csvtest2 (intr2, strr2)
VALUES (3, "r2_3");
INSERT INTO csvtest2 (intr2, strr2)
VALUES (4, "r2_4");

DROP TABLE IF EXISTS csvtest3;
CREATE TABLE csvtest3
(
  intr3 INT PRIMARY KEY,
  strr3 VARCHAR(20)
);

INSERT INTO csvtest3 (intr3, strr3)
VALUES (1, "r3_1");
INSERT INTO csvtest3 (intr3, strr3)
VALUES (2, "r3_2");
INSERT INTO csvtest3 (intr3, strr3)
VALUES (3, "r3_3");
INSERT INTO csvtest3 (intr3, strr3)
VALUES (4, "r3_4");

DROP TABLE IF EXISTS csvtest4;
CREATE TABLE csvtest4
(
  intr4 INT PRIMARY KEY,
  doubler4 DOUBLE,
  decimalr4 DECIMAL(11,2)
);

INSERT INTO csvtest4 (intr4, doubler4, decimalr4)
VALUES (1, 11.1, 111.11);
INSERT INTO csvtest4 (intr4, doubler4, decimalr4)
VALUES (2, 22.2, 222.22);
INSERT INTO csvtest4 (intr4, doubler4, decimalr4)
VALUES (3, 33.3, 333.33);
INSERT INTO csvtest4 (intr4, doubler4, decimalr4)
VALUES (4, 44.4, 444.44);
