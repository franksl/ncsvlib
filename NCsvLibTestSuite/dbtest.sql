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