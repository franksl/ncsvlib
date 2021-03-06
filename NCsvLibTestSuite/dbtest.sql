DELIMITER //
DROP PROCEDURE IF EXISTS CreateTestDb//
CREATE PROCEDURE CreateTestDb()
BEGIN

  DROP TABLE IF EXISTS csvtest1;
  CREATE TABLE csvtest1
  (
    intfld INT PRIMARY KEY,
    strfld VARCHAR(20),
    doublefld DOUBLE,
    decimalfld DECIMAL(11,2),
    dtfld DATETIME,
    strfld2 VARCHAR(5),
    boolfld INT
  );

  INSERT INTO csvtest1(intfld, strfld, doublefld, decimalfld, dtfld, strfld2, boolfld)
  VALUES (1, 'aaa', 100.10, 1000.11, '2001-1-11 10:11:12', 'TEST1', 1);
  INSERT INTO csvtest1(intfld, strfld, doublefld, decimalfld, dtfld, strfld2, boolfld)
  VALUES (2, 'bbb', 200.20, 2000.22, '2002-2-12 12:13:14', 'TEST2', 0);
  INSERT INTO csvtest1(intfld, strfld, doublefld, decimalfld, dtfld, strfld2, boolfld)
  VALUES (3, 'ccc', 300.30, 3000.33, '2003-3-13 13:14:15', 'TEST3', 1);
  INSERT INTO csvtest1(intfld, strfld, doublefld, decimalfld, dtfld, strfld2, boolfld)
  VALUES (4, 'ddd', 400.40, 4000.44, '2004-4-14 14:15:16', 'TEST4', 0);

  DROP TABLE IF EXISTS csvtest2;
  CREATE TABLE csvtest2
  (
    intr2 INT PRIMARY KEY,
    intr2left INT,
    strr2 VARCHAR(20),
    bool2 CHAR(1)
  );

  INSERT INTO csvtest2 (intr2, intr2left, strr2, bool2)
  VALUES (1, 11, 'r2_1', 'T');
  INSERT INTO csvtest2 (intr2, intr2left, strr2, bool2)
  VALUES (2, 22, 'r2_2', 'F');
  INSERT INTO csvtest2 (intr2, intr2left, strr2, bool2)
  VALUES (3, 33, 'r2_3', 'T');
  INSERT INTO csvtest2 (intr2, intr2left, strr2, bool2)
  VALUES (4, 44, NULL, 'F');
  INSERT INTO csvtest2 (intr2, intr2left, strr2, bool2)
  VALUES (5, 55, 'r2_5', 'T');
  INSERT INTO csvtest2 (intr2, intr2left, strr2, bool2)
  VALUES (6, 66, 'r2_6', 'F');
  INSERT INTO csvtest2 (intr2, intr2left, strr2, bool2)
  VALUES (7, 77, 'r2_7', 'T');
  INSERT INTO csvtest2 (intr2, intr2left, strr2, bool2)
  VALUES (8, 88, 'r2_8', 'F');
  INSERT INTO csvtest2 (intr2, intr2left, strr2, bool2)
  VALUES (9, 99, 'r2_9', 'F');
  INSERT INTO csvtest2 (intr2, intr2left, strr2, bool2)
  VALUES (10, 100, 'r2_10', 'F');
  INSERT INTO csvtest2 (intr2, intr2left, strr2, bool2)
  VALUES (11, 110, 'r2_11', 'F');

  DROP TABLE IF EXISTS csvtest3;
  CREATE TABLE csvtest3
  (
    intr3 INT PRIMARY KEY,
    strr3 VARCHAR(20)
  );

  INSERT INTO csvtest3 (intr3, strr3)
  VALUES (1, 'r3_1');
  INSERT INTO csvtest3 (intr3, strr3)
  VALUES (2, 'r3_2');
  INSERT INTO csvtest3 (intr3, strr3)
  VALUES (3, 'r3_3');
  INSERT INTO csvtest3 (intr3, strr3)
  VALUES (4, 'r3_4');
  INSERT INTO csvtest3 (intr3, strr3)
  VALUES (5, 'r3_5');
  INSERT INTO csvtest3 (intr3, strr3)
  VALUES (6, 'r3_6');
  INSERT INTO csvtest3 (intr3, strr3)
  VALUES (7, 'r3_7');
  INSERT INTO csvtest3 (intr3, strr3)
  VALUES (8, 'r3_8');
  INSERT INTO csvtest3 (intr3, strr3)
  VALUES (9, 'r3_9');
  INSERT INTO csvtest3 (intr3, strr3)
  VALUES (10, 'r3_10');
  INSERT INTO csvtest3 (intr3, strr3)
  VALUES (11, 'r3_11');
  INSERT INTO csvtest3 (intr3, strr3)
  VALUES (12, 'r3_12');

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
  INSERT INTO csvtest4 (intr4, doubler4, decimalr4)
  VALUES (5, 55.5, 555.55);
  INSERT INTO csvtest4 (intr4, doubler4, decimalr4)
  VALUES (6, 66.6, 666.66);
  INSERT INTO csvtest4 (intr4, doubler4, decimalr4)
  VALUES (7, 77.7, 777.77);
  INSERT INTO csvtest4 (intr4, doubler4, decimalr4)
  VALUES (8, 88.8, 888.88);
  INSERT INTO csvtest4 (intr4, doubler4, decimalr4)
  VALUES (9, 99.9, 999.99);
  
  DROP TABLE IF EXISTS csvtest5;
  CREATE TABLE csvtest5
  (
    intr5 INT PRIMARY KEY,
    strr5 VARCHAR(10)
  );
  
  INSERT INTO csvtest5 (intr5, strr5)
  VALUES (1, 'AA');
  INSERT INTO csvtest5 (intr5, strr5)
  VALUES (2, 'BB');
  INSERT INTO csvtest5 (intr5, strr5)
  VALUES (3, 'CC');
  INSERT INTO csvtest5 (intr5, strr5)
  VALUES (4, 'DD');
  INSERT INTO csvtest5 (intr5, strr5)
  VALUES (5, 'EE');
  INSERT INTO csvtest5 (intr5, strr5)
  VALUES (6, 'FF');
  INSERT INTO csvtest5 (intr5, strr5)
  VALUES (7, 'GG');
  INSERT INTO csvtest5 (intr5, strr5)
  VALUES (8, 'HH');
  
  DROP TABLE IF EXISTS csvtest6;
  CREATE TABLE csvtest6
  (
    intr6 INT PRIMARY KEY,
    strr6 VARCHAR(10)
  );
  
  INSERT INTO csvtest6 (intr6, strr6)
  VALUES (11, 'AAA');
  INSERT INTO csvtest6 (intr6, strr6)
  VALUES (22, 'BBB');
  INSERT INTO csvtest6 (intr6, strr6)
  VALUES (33, 'CCC');
  INSERT INTO csvtest6 (intr6, strr6)
  VALUES (44, 'DDD');
  INSERT INTO csvtest6 (intr6, strr6)
  VALUES (55, 'EEE');
  INSERT INTO csvtest6 (intr6, strr6)
  VALUES (66, 'FFF');
  INSERT INTO csvtest6 (intr6, strr6)
  VALUES (77, 'GGG');
  INSERT INTO csvtest6 (intr6, strr6)
  VALUES (88, 'HHH');
  
END//
DELIMITER ;