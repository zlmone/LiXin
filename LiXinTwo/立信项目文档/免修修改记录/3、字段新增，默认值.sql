------------------第一步---------------------------
ALTER TABLE Sys_User
ADD CPACreateDate DATETIME NULL，
    IsHistoryNew INT,--历史上是新进员工
    NewYear INT--成为新进员工的年度


------------------第二步---------------------------
UPDATE Sys_User
SET IsHistoryNew=1,NewYear=2013
WHERE IsNew=1
