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

![Règles 2D](https://github.com/vclimpont/plant-generation/blob/main/Images/regles2d.PNG)

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
* ‘+’ : Ajoute une valeur *alpha* définie à l’angle de rotation *theta* selon l'axe *x*
* ‘-’ : Soustrait une valeur *alpha* définie à l’angle de rotation *theta* selon l'axe *x*
* ‘[‘ : Enregistre son état actuel dans la liste d’états
* ‘]’ : Instancie une feuille, retourne au dernier état enregistré dans la liste

En déterminant un ensemble de règles cohérent, notamment dans sa structure de modification des états et des rotations, ainsi qu’un faible angle *alpha* de rotation, il est rapidement possible d’obtenir une structure organique semblable à un arbre.

Exemple d'un arbre en 2D :

![Arbre 2D](https://github.com/vclimpont/plant-generation/blob/main/Images/arbre2d.PNG)

## Génération des végétaux
Les règles établies pour le fonctionnement de la tortue soulèvent intrinsèquement des problématiques de déplacements dans le plan et l'espace. La résolution de ces problèmes est basée sur la détermination, pour chaque nouveau déplacement, d'un système de coordonnées cartésiennes dans 2 et 3 dimensions.
### 1. Dans le plan (2D)
Lors d'un déplacement dans le plan, la position d'arrivée *(x, y)* de la tortue est ajoutée à sa position actuelle *(xp, yp)* telle que *(x, y)* :

![Coords 2D](https://github.com/vclimpont/plant-generation/blob/main/Images/coord2d.PNG)
![Plane](https://github.com/vclimpont/plant-generation/blob/main/Images/plane.PNG)

avec : 
* *r* = la valeur de translation actuelle de la tortue
* *theta* = la valeur de l'angle de rotation actuelle de la tortue
### 2. Dans l’espace (3D)
Lors d'un déplacement dans l'espace, l'axe *z* est ajouté à l'équation. La position d'arrivée *(x, y, z)* de la tortue est ajoutée à sa position actuelle *(xp, yp, zp)* telle que *(x, y, z)* :

![Coords 3D](https://github.com/vclimpont/plant-generation/blob/main/Images/coord3d.PNG)
![Space](https://github.com/vclimpont/plant-generation/blob/main/Images/space.PNG)

avec :
* *rho* = la valeur de translation actuelle de la tortue
* *theta* = la valeur de l'angle de rotation actuelle de la tortue selon l'axe *x*
* *phi* = la valeur de l'angle de rotation actuelle de la tortue selon l'axe *z*

Dans le cadre d'une génération de végétaux en trois dimensions, de nouvelles valeurs d'interprétation sont ajoutées aux actions de la tortue :
* ‘&’ : Ajoute une valeur *beta* définie à l’angle de rotation *phi* selon l'axe *z*
* ‘^’ : Soustrait une valeur *beta* définie à l’angle de rotation *phi* selon l'axe *z*

![Règles 3D](https://github.com/vclimpont/plant-generation/blob/main/Images/regles3d.PNG)

Exemple d'un arbre en 3D :

![Arbre 2D](https://github.com/vclimpont/plant-generation/blob/main/Images/arbre3d.PNG)

## Variation des végétaux
Toutes les générations de végétaux issues d'une même espèce ; autrement dit, toutes les générations d'un LSystem qui suivent un même ensemble de règles, sont pour le moment identiques. Dans l'optique d'introduire des variations de structure, tout en conservant l'aspect général du specimen généré, le projet inclut deux types de randomisation : une variation des règles et une variation du comportement de la tortue.
### 1. LSystems stochastiques
L'idée générale du LSystem stochastique consiste à varier la substitution de caractères au sein même des règles établies. 
Ainsi, la règle de substitution de chaque caractère est remplacée par un ensemble de règles différentes, qui conservent toutefois une structure cohérente.

Exemple de règles stochastiques : 

![Règles stochastiques](https://github.com/vclimpont/plant-generation/blob/main/Images/reglesstochastiques.PNG)

Lorsqu'un caractère doit être substitué, une chaîne de caractères est piochée de manière aléatoire dans son ensemble de règles avec des probabilités équivalentes. (1/3 dans l'exemple ci-dessus). 
Cette méthode permet de conserver l'aspect général de l'espèce générée en incluant des modifications structurelles : 

![Arbre type 3 1](https://github.com/vclimpont/plant-generation/blob/main/Images/type31.PNG)
![Arbre type 3 2](https://github.com/vclimpont/plant-generation/blob/main/Images/type32.PNG)

### 2. Variance des angles
La seconde méthode consiste à inclure une variation aléatoire des angles de rotation *theta* et *phi* dans le déplacement de la tortue.
Ainsi, à chaque interprétation des caractères **+ - &** et **^**, correspondant aux modifications des angles de rotation de la tortue : 
soit *v*, une variance donnée dans l'intervalle [0, 1] et *alpha*, la valeur de rotation établie initialement :

  *delta_rotation = alpha + random_between ( -alpha * v , alpha * v)* 
  
Cette méthode permet de conserver une structure identique pour chacune des espèces de végétaux, en incluant des variations dans l'écartement des branches :

![Arbre type 3 1 angle](https://github.com/vclimpont/plant-generation/blob/main/Images/type31angle.PNG)
![Arbre type 3 2 angle](https://github.com/vclimpont/plant-generation/blob/main/Images/type32angle.PNG)
## Optimisation des performances

Lors de la génération d'un arbre, chaque déplacement de la tortue instancie un nouvel objet dans la scène. Chacune des branches et des feuilles d'une espèce générée constitue un objet unique. Un arbre complet représente ainsi en moyenne entre 10 000 et 15 000 meshes.

Dans l'optique de résoudre ces problématiques de performances, le projet se base sur la réécriture de la méthode *CombineMeshes* de Unity. L'idée consiste à combiner les meshes d'un arbre en un seul *MeshFilter*. 
Cependant, afin d'éviter les restrictions de nombre de vertices d'un mesh imposé par Unity, qui pourrait causer des problèmes lors de la génération d'un grand arbre ; le procédé de fusion des meshes d'un arbre est établi comme suit :

Toutes les branches et feuilles de l'arbre instanciées sont réparties de manière équilibrée dans 10 sous-parents *branchParent* et 10 sous-parents *leafParent*.
Puis, pour chacun des sous-parents, l'ensemble des meshes des enfants est fusionné en un unique *MeshFilter* ajouté au sous-parent.
Le nombre de meshes instanciés à chaque arbre passe ainsi de 10 000~15 000 à 20 (un pour chaque sous-parent).

**Résultats :**

Sans fusion | Avec fusion
------------ | -------------
2 arbres | 200 arbres
10 FPS | 30 FPS

Scène de 200 arbres :

![scene](https://github.com/vclimpont/plant-generation/blob/main/Images/scene.PNG)

## Sources

Explication des LSystem :

https://www.youtube.com/watch?v=E1B4UoSQMFw

https://fr.wikipedia.org/wiki/L-Syst%C3%A8me

Structure des branches :

http://algorithmicbotany.org/papers/abop/abop-ch1.pdf (partie 1.6.3)

Déplacement dans l’espace :

http://algorithmicbotany.org/papers/abop/abop-ch1.pdf (partie 1.5)

https://www.bioquest.org/products/files/13157_Real-time%203D%20Plant%20Structure%20Modeling%20by%20L-System.pdf (partie 3)

http://sites.science.oregonstate.edu/math/home/programs/undergrad/CalculusQuestStudyGuides/vcalc/coord/coord.html

Variation du LSystem :

http://algorithmicbotany.org/papers/abop/abop-ch1.pdf (partie 1.7)

Optimisation des performances : 

https://docs.unity3d.com/ScriptReference/Mesh.CombineMeshes.html

https://www.youtube.com/watch?v=NcmPz_nbArY


