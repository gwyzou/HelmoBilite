# **HELMO-BILITE**

## US1. vitrine

1. présentation de la flotte
2. presetnation de l'équipe
3. présentation du savoir faire
4. liste des clients (logo) >>> mur de logo

## US2.crération de compte

- client
- HELMo
    - chauffeur | dispatcher

> demande au moment du clique sur 'inscription'

- si client
    - company name
    - adresse siège social
    - email de contact
    - passwd + confirmation passwd
- HELMo --> Chauffeur
    - matricule
    - nom
    - prénom
    - mail
    - passwd + confirmation
    - type de permis (possibilité de plusieur voir tous)
        - B
        - C (Camion)
        - CE (Camion avec remorque)


- HELMo --> Dispatcher
    - matricule
    - nom
    - prénom
    - mail
    - passwd + confirmation
    - niveau d'étude
        - cess
        - bac
        - licence

## US3. Connexion

- matricule / email + passwd

## US4. Modification Compte

- ajout birth date ++ profil picture (HELMO)
    - afficher le type de permis
- ajout profil pic = logo entreprise (CLIENT)

## US5. Encodage demande livraison ***(CLIENT)***

- need
    - lieu chargement
    - Date & Heure Voulue
    - lieu déchargement (multiple ?)
    - Date & Heure Voulue
    - Contenu de livraison

## US6. Modification demande livraison ***(CLIENT)***

- la demande n'a pas encore été acceptée
    - sinon c'est ciao

## US7. Visualisation des demandes livraison(s) ***(CLIENT)***

- affiche les livraisons **TERMINÉE**

## US8. Assigner une livraison à un chauffeur ***(DISPATCHER)***

- afficher les demandes pas encore assignées
- sur sélection de demande
    - affichage et sélection d'un chauffeur **LIBRE** à la date voulue
    - affichage et sélection d'un camion **LIBRE** à la date demandée (/!\ le chauffeur doit avoir le bon permis)
- marge de 60 minutes (avant **&** après) l'heure voulue pour le chragement (***VOIR FIN DE DOCUMENTATION***)
    - si personne n'est dispo alors message d'info au dispatcher

## US9. Rafraichir la liste des demandes ***(DISPATCHER)***

- un bouton pour rafraichir la liste des demandes / un timer pour rafraichir la liste des demandes

## US10. Visualiser la liste des livriaisons assignées ***(CHAUFFEUR)***

- sur la semaine en cours
- bonus si sous format calendrier

## US11. Valider une livraison ***(CHAUFFEUR)***

- click sur boutton "Livraison Effectuée"
- commentaire possible

## US12. Marquer une commande comme non livrée ***(CHAUFFEUR)***

- click sur boutton "Livraison Ratée"
- choix de la raison dans la liste **prédéfinie**
    - Maladie
    - Accident (lors du **TRAJET**)
    - Client Absent / Livraison Impossible

  > cette liste peut être élargie

## US13. Connexion Admin

- ne change pas d'une connexion normal (**pour l'instant**)

## US14. Ajouter / Supprimer / Visualiser des permis ***(ADMIN)***

- minimum de ***1*** permis
- /!\ si un permis est supprimé alors il faut vérifier que les chauffeurs qui l'ont ne sont pas assignés à une livraison
    - si oui alors livraison retourne en "attente d'assignation"

## US15. Ajouter / Supprimer / Visualiser des camions ***(ADMIN)***

- /!\ de bien charger **TOUTES** les infos du camion lors de la modification / création (
  ***VOIR FIN DE DOCUMENTATION***)

## US16. Lister ***TOUTES*** (= passée + présent + future ) les demandes livraison ***(ADMIN)***

- future = date de livraison > date du jour
- présent = date de livraison = date du jour &| livraison validée mais pas encore terminée
- passé = date de livraison < date du jour

## US17. Tagguer un client comme *Mauvais payeur* ***(ADMIN)***

- les commmandes du client en cours ( = assignée à un chauffeur) seront exécutées mais les suivantes (= non assignée)
  seront grisées

## US18. Visualiser les statistiques ***(ADMIN)***

- par chauffeur
- par date
- par camion
- champ de recherche / tri pour filtrer les résultats
    - champs de recherche avec autocomplétion

---

## Documentation

### Initialisation DataBase

- +10 camions de != types (c / CE)
- +5 chauffeurs avec les != permis
- +2 dispatchers
- +5 clients
- +10 commandes factices
- +1 admin

### Camion

- Marque
    - liste
- Modèle
- Immatriculation
    - respect immatriculation belge (= 1 chiffre '-' 3 lettres '-' 3 chiffres)
- Type
    - C = Camion
    - CE = Camion avec remorque
- Tonnage max ( = charge utile)
- Photo

