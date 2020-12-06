# Génération procédurale d'une végétation sous Unity

Cette section présente et explique les différentes étapes de développement d’un système de génération procédurale de végétaux sous Unity. L’objectif du projet est de permettre la génération d’arbres uniques de mêmes espèces, par action de clic de l’utilisateur dans l’espace de construction

## Présentation du LSystem
### 1. Structure du système
La méthode de construction des arbres choisie pour ce projet repose principalement sur l’utilisation d’un LSystem. Il s’agit d’un système de grammaire formelle qui comprend :
* Un alphabet :  l'ensemble des variables du système.
* Un ensemble de valeurs d’interprétation.
* Un axiome de départ, vu comme l'état initial.
* Un ensemble de règles de réécriture.

Le principe de cette méthode est, de manière récursive et suivant un alphabet donné, de substituer l’état *n* à l’état *n + 1* en suivant un ensemble de règles de réécriture. 

Exemple d’une structure LSystem utilisée dans le projet : 

![Règles 2D](https://github.com/https://github.com/vclimpont/plant-generation/main/Images/regles2d.PNG)

Depuis l’état initial “*X*”, le système substitue à chaque itération tous les caractères “X” et “F” de la chaîne de caractères de l’état actuel par leur équivalent défini dans l’ensemble des règles. 

### 2. Interprétation de la tortue

Au terme de toutes les itérations, l’état final du LSystem produit une chaîne de caractères qui sert ensuite à l’interprétation dite de la “tortue”. L’objectif est de substituer chaque caractère de la chaîne finale à une action de mouvement de l’entité tortue, chargée ainsi de dessiner progressivement une forme en se déplaçant dans le plan ou dans l’espace. 

L’entité tortue possède :
* Une position dans l’espace
* Une valeur de translation (prochaine distance à parcourir)
* Une largeur (largeur de l’objet qu’elle dessine en se déplaçant)
* Une valeur d’angle theta (valeur de rotation pour le prochain déplacement)
* Une liste d’états de la tortue sauvegardés

Pour chaque caractère de la chaîne de caractères finale obtenue, l’interprétation s’effectue telle que :
* ‘F’ : Se déplace, instancie une nouvelle branche et réduit la largeur des prochaines branches d’un facteur *f*.
* ‘+’ : Ajoute une valeur alpha définie à l’angle de rotation *theta*
* ‘-’ : Soustrait une valeur alpha définie à l’angle de rotation *theta*
* ‘[‘ : Enregistre son état actuel dans la liste d’états
* ‘]’ : Instancie une feuille, retourne au dernier état enregistré dans la liste

En déterminant un ensemble de règles cohérent, notamment dans sa structure de modification des états et des rotations, ainsi qu’un faible angle *alpha* de rotation, il est rapidement possible d’obtenir une structure organique semblable à un arbre.

## Génération des végétaux
### 1. Dans le plan (2D)
### 2. Dans l’espace (3D)
## Variation des végétaux
### 1. LSystems stochastiques
### 2. Variance des angles
## Optimisation des performances
