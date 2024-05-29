 -- update pret 
 -- set nbr_jour_retard = case  
--	when pret.jour_retour_effectif is null 
--			then DATEDIFF(day, pret.jour_retour_prevue, GETDATE())
-- else 
--	DATEDIFF(day, pret.jour_retour_prevue , pret.jour_retour_effectif )
-- end
-- where pret.membre = 2;
---------------------------------------------
declare @nom varchar(200) = 'karim';
select nom , prenom from membre where lower(nom)=@nom or lower(prenom)=@nom;
