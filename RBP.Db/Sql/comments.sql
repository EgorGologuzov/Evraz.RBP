UPDATE public."Statements"
	SET "Comment" = NULL;

UPDATE public."Statements" AS s
	SET "Comment" = '���� ��������'
	WHERE (SELECT COUNT(*) FROM public."StatementDefects" WHERE "StatementId" = s."Id") <> 0;