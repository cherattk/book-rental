-- update pret set jour_retour_effectif=GETDATE() where pret.id=4;
-- update pret set jour_retour_effectif=null where pret.id=3;
-- update pret set livre = 23 where id = 11;
-- SELECT * FROM pret left join livres on pret.livre = livres.id where membre = 2;
-- delete from pret where id > 15;
select * from pret;
-- select id from livres;