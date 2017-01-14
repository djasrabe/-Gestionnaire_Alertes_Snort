"#-Gestionnaire_Alertes_Snort" 

Mon projet de fin d'etude en Master Réseaux ( Maintenant je refais un master en programmation).

------------------------------------------------

Ce projet consistait à automatiser les traitements des alertes generées par un detecteur d'intrusion Snort IDS et un seveur Syslog
(Que nous avons installés) en ecoute sur le reseau de l'entreprise et stockés dans un BD MySQL.

Le role de l'application:

Traitement en temps reel des alertes et prise de decision à la volée en fonction de la gravité de l'alerte
(gravité fixée suivant le Sid de la base des signatures Snort):

Envoyer un mail à l'admin ----
Envoyer un SMS à l'admin (Modem GSM connecté et commandé par l'Appli)---

Envoyer une commande de config ACL au routeur pour bloquer instantanemment l'@ ip en cause

ou Faire les trois
                                                                                            
                                                                                            
