------------------��һ��---------------------------
ALTER TABLE Sys_User
ADD CPACreateDate DATETIME NULL��
    IsHistoryNew INT,--��ʷ�����½�Ա��
    NewYear INT--��Ϊ�½�Ա�������


------------------�ڶ���---------------------------
UPDATE Sys_User
SET IsHistoryNew=1,NewYear=2013
WHERE IsNew=1
