<div style="text-align:center"><p align="center"><img src ="https://i.imgur.com/zA3Xh1a.png" /></p></div>

# SCL-19-T3-A : Learn CTS

Projet d'intégration des allochtones souhaitant s'initier à la citoyenneté française et plus particulièrement à travers les transports en communs.

## Présentation rapide du logiciel

Vous avez bien lu !
Contrairement aux apparences, "Learn CTS" n'est pas un jeu à proprement parler mais bien un **logiciel**.
En effet, il s'agit d'un **éditeur**  à plusieurs **moteurs de jeu**.

Il est donc à destination de tous ceux souhaitant créer un **jeu sérieux** sans avoir à le programmer soi-même !

Celui-ci propose en effet de nombreux **outils** d'édition, un **accompagnement** sous la forme d'aides et des **exemples**. Étant initialement créé à des fins d'**éducation au langage**, ce logiciel permet un suivi et une pédagogie qui n'a pour limite que les idées de son utilisateur !

Afin de faciliter le travail de création, les jeux créés sont divisés en **scénarios** et en ***situations**.

## Documentation

[Lien d'accès à la documentation](https://webetu.iutrs.unistra.fr/~lallemann/doc-cts/learn-cts/Learn%20CTS/html/namespace_learn___c_t_s.html) (accessible uniquement depuis l'IUT)

## Composition d'un jeu

Un jeu est tout d'abord composé de **scénarios**. Ces scénarios seront séléctionnables au lancement du jeu. Ils peuvent donc être utilisés comme des niveaux différents, mais aussi comme des niveaux de difficulté.

Dans un scénario, on trouve les **situations**. Contrairements aux scénarios, le joueur ne peut pas choisir quel situation jouer. Pour accéder à la situation suivante, il faut **valider la situation précédente**.

**Alors qu'est-ce qu'on trouve dans une situation ?**

✅ Des figurants à l'apparence entièrement personnalisable, pour rendre la situation encore plus immersive.

✅ Des interactions avec des personnes, des objets, des animaux... Tout est possible !

✅ De nombreux environnements et personnages disponibles, à utiliser directement sans s'embêter !

✅ Un tram entièrement fonctionnel pour introduire le joueur à une situation quotidienne dans la ville de strasbourg.

✅ Un jeu démo, pour explorer les possibilités de Learn CTS. Si une siuation vous plaît, vous n'aurez qu'à la copier dans votre jeu !

✅ Des **dialogues personnalisables** et de l'**audiodescription** pour s'entraîner à la **compréhension écrite et orale**.

✅ Des outils pour le joueur, tels qu'un sac avec un téléphone.

<table align="center">
    <tr>
        <td><img src ="https://i.imgur.com/XfnvfAl.png" width="500" height="300" /></td>
        <td><img src ="https://i.imgur.com/6a5uw1S.png" width="500" height="300" /></td>
    </tr>
    <tr>
        <td>Première situation : Vue à la première personne, le joueur achète son ticket.</td>
        <td>Deuxième situation : Le joueur valide son ticket et monte dans le tram.</td>
    </tr>
</table>

### Les moteurs de jeu

Learn CTS offre l'accès à plusieurs **moteurs de jeu**, pour donner plus de possibilités au créateur. 

Ces moteurs ont tous un point commun : Le déroulement est sous la forme d'**interaction question-réponse**. Ce système permet de créer des **dialogues complexes**, qui n'ont pas de limite de longueur.

Chaque situation a la possibilité d'être liée à un moteur de jeu différent, pour un maximum de **diversité** dans le gameplay.

## Comment créer son jeu ?

### Choix ou création d'un nouveau jeu

Une fois sur le menu principal, cliquez sur **Mes jeux**. Editez un jeu déjà existant en cliquant sur le crayon <img src="https://i.imgur.com/JAbIO2D.png" width="32" height="32"/>d'un jeu, ou créez un nouveau jeu avec le bouton <img src="https://i.imgur.com/TY0AKSZ.png" width="32" height="32"/>.

**Cette action va lancer l'éditeur de jeu.**

### Général

La première rubrique de l'éditeur est **Général** Vous pourrez y modifier le titre et la description du jeu.

Comme tous les textes changeables du jeu, il faut **valider le changement** en appuyant sur la touche "Entrée". Vous pouvez aussi annuler le changement avec la touche "Echap".

### Modèles

Dans cette rubrique, il est possible de modifier et de créer des **figurants**, des **dialogues**, ainsi que les images des **personnages** et des **décors**.


#### Figurants

Ajoutez un figurant en cliquant sur le bouton <img src="https://i.imgur.com/TY0AKSZ.png" width="32" height="32"/> ou supprimez-en en cliquant sur la croix <img src="https://i.imgur.com/epcD5so.png" width="32" height="32"/>.

Choisissez l'apparence de votre figurant avec les flèches, renommez-le en cliquant sur son nom.

#### Dialogues

**Dans ce menu, vous pouvez créer des dialogues, une question à la fois, et les assigner à des personnages.**

Vous pouvez créer une nouvelle question en cliquant sur le bouton <img src="https://i.imgur.com/TY0AKSZ.png" width="32" height="32"/>.

Chaque question, numérotée dans un onglet en haut à gauche du cadre du dialogue, est composée de **un ou plusieurs choix de réponse**.

Changez le texte de la question en cliquant dessus, ajoutez un choix de réponse avec le bouton <img src="https://i.imgur.com/p0VrcMo.png" width="32" height="32"/>, et choisissez comment la question sera présentée au joueur : uniquement sous forme de **texte**, d'**audio**, ou **les deux** ?

Pour chaque **choix de réponse**, personnalisable en cliquant sur le texte, vous pouvez assigner un **score** positif ou négatif. Il est possible de **choisir une question sur laquelle rediriger le joueur** s'il choisit cette réponse.
Cette redirection permet des dialogues complexes et riches, qui augmente la personnalisation du jeu et l'immersion du joueur.

Supprimez une question (et ses choix de réponse avec, attention !) en cliquant sur la croix <img src="https://i.imgur.com/epcD5so.png" width="32" height="32"/>, ou supprimez un choix de réponse unique en cliquant sur la poubelle <img src="https://i.imgur.com/dDG9pNU.png" width="32" height="32"/>.
Ne vous perdez pas, les questions sont réparties sur des pages, que vous pouvez changer avec les flèches en haut à droite de l'écran.

### Images

La rubrique Images permet d'ajouter ou des supprimer des **images utilisables directement dans votre éditeur**, et donc dans votre jeu !

#### Personnages

Ce menu donne l'accès aux images des personnages, et de leur animation. Vous pouvez ajouter un nouvel ensemble d'images pour un personnage en cliquant sur le bouton <img src="https://i.imgur.com/TY0AKSZ.png" width="32" height="32"/>.

Chaque ensemble d'images est placé dans un cadre, qui représente l'animation d'un personnage à chaque fois. La croix <img src="https://i.imgur.com/epcD5so.png" width="32" height="32"/> permet de supprimer l'ensemble d'images. La première ligne d'images contient les animations de déplacement vers la droite du personnage, et la deuxième ligne les animations vers la gauche.

En cliquant sur une image, l'explorateur de fichier de votre ordinateur s'ouvre. Vous pouvez alors choisir une image, qui sera uilisée pour une étape de l'animation. L'icone en bas à gauche du cadre vous indique si les images sont valides ou non.

### Scénarios

Cette rubrique vous permet d'ajouter des scénarios. Quand vous ajoutez un scénario, il est créé vide. Pour ajouter une nouvelle situation dans ce scénario, il suffit de cliquer sur **Ajouter une nouvelle situation**.

Vous pouvez voir le nom complet du scénario, avec son arborescence, dans l'onglet en haut à gauche du cadre. Juste en dessous, vous pouvez modifier l'ordre d'apparition du scénario avec les flèches pointant vers le haut et vers le bas. Pour renommer le scénario, il suffit de cliquer sur son nom ou sur le crayon à coté. Vous pouvez supprimmer le scénario en cliquant sur la poubelle <img src="https://i.imgur.com/dDG9pNU.png" width="32" height="32"/> à coté. 

### Les situations

La situation est le **niveau de jeu dans lequel va évoluer le joueur**. Vous pouvez, comme pour les scénarios, modifier l'ordre d'apparition de la situation. Notez que celui-ci est limité au scénario de la situation.

La situation est introduite par un **écran d'introduction**. Vous pouvez y renseigner le **lieu de déroulement de la situation**, et un **texte d'introduction**.

La **densité de PNJs** est réglable par un curseur horizontal. Plus elle est faible, moins il y aura de PNJs dans la situation, et inversement.

Vous pouvez choisir un **décor**, défini plus haut dans la rubrique **Images/Décors**. Vous pouvez alors séléctionner le décor par son nom, qui est le nom du fichier de l'image.

Le **type de scène** est le moteur de jeu utilisé pour la situation. Un trajet en tram, une marche dans la ville ou dans un parc, une vision à la première personne, et bien d'autres sont déjà présents dans Learn CTS. Choisissez celui qui convient le mieux à vos envies !

La rubrique **Evènements** permet de **mettre en lien un Figurant avec un dialogue** définit dans la rubrique **Modèles/Figurants** et **Modèles/Dialogues** respectivement.

Vous pouvez ajouter un évènement par figurant avec le bouton <img src="https://i.imgur.com/TY0AKSZ.png" width="32" height="32"/>. La liste déroulante de gauche permet le choix du figurant voulu, et la liste déroulante de droite permet le choix du dialogue qui lui sera associé pendant la partie.

Le bouton **Placer** ouvre une nouvelle fenêtre qui permet de **placer le figurant dans le décor**, selon vos souhaits. Il est placé par défaut en haut à gauche de la fenêtre. Pour le **déplacer** le figurant, il faut cliquer dessus et maintenir le clic enfoncé jusqu'à ce que la position vous convient. Si l'emplacement que vous avez choisi est jugé invalide par l'éditeur, celui-ci vous préviendra et placera le figurant aléatoirment dans le décor.

**Pour jouer un aperçu de la situation cliquez sur le bouton <img src="https://i.imgur.com/E3ppsKZ.png" width="32" height="32"/>. 

###### C'est tout !