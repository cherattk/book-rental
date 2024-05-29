insert into retard(pret, nbr_jour_retard, montant)
select pretId, jourRetard, jourRetard*(10) as montant 
from( 
(select pret.id as pretId, DATEDIFF(day, pret.jour_retour_prevue, GETDATE()) as jourRetard 
from pret where DATEDIFF(day, pret.jour_retour_prevue, GETDATE()) > 0)  ) as caculeRetard  
where not exists(select pret from retard where retard.pret = 3);
