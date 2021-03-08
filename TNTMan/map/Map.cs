using System;
using System.Collections.Generic;
using System.Drawing;
using TNTMan.entitees;
using TNTMan.map.blocs;

namespace TNTMan.map
{
    class Map
    {
        public static readonly int LARGEUR_GRILLE = 15;
        public static readonly int LONGUEUR_GRILLE = 11;

        int id;
        Bloc[,] listeBlocs;
        List<Entite> listeEntites;

        public int getId()
        {
            return id;
        }

        public void chargerMapParDefaut()
        {
            int x, y;
            listeBlocs = new Bloc[LARGEUR_GRILLE, LONGUEUR_GRILLE];
            for(x = 0; x < LARGEUR_GRILLE;x++)
            {
                listeBlocs[x, 0] = new BlocIncassable();
                listeBlocs[x, LONGUEUR_GRILLE - 1] = new BlocIncassable();
            }
            for (y = 0; y < LONGUEUR_GRILLE; y++)
            {
                listeBlocs[0, y] = new BlocIncassable();
                listeBlocs[LARGEUR_GRILLE - 1, y] = new BlocIncassable();
            }
            for (x = 2; x < LARGEUR_GRILLE-1; x+=2)
                for (y = 2; y < LONGUEUR_GRILLE - 1; y+=2)
                {
                    listeBlocs[x, y] = new BlocIncassable();
                }

            for(int i = 0;i<60;)
            {
                x = Program.random.Next(1, LARGEUR_GRILLE - 1);
                y = Program.random.Next(1, LONGUEUR_GRILLE - 1);

                // on ne génére pas dans les coins de la grille
                if((x < 3 && y < 3)
                || (x < 3 && y > LONGUEUR_GRILLE - 4)
                || (x > LARGEUR_GRILLE - 4 && y < 3)
                || (x > LARGEUR_GRILLE - 4 && y > LONGUEUR_GRILLE - 4))
                    continue;

                // on ne change pas les blocs déjà générés
                if (listeBlocs[x, y] != null)
                    continue;

                listeBlocs[x, y] = new BlocTerre();
                i++;
            }
            listeEntites = new List<Entite>();
        }

        public void dessinerToutesLesEntites(IntPtr rendu)
        {
            listeEntites.ForEach((e) => {
                if (!e.estMort())
                {
                    e.dessiner(rendu);
                }
            });
        }

        public void mettreAjourToutesLesEntites()
        {
            listeEntites.ForEach((e) => {
                if (!e.estMort())
                {
                    e.mettreAJour(this);
                }
            });
            listeEntites.RemoveAll((e) => e.estMort());
        }

        public Bloc getBlocA(int x, int y)
        {
            return listeBlocs[x, y];
        }

        public static Point getPositionEcranDepuis(float x, float y, int w=32, int h=32)
        {
            Size resolution = Gfx.getResolution();
            Size tailleGrille = new Size(Map.LARGEUR_GRILLE * Bloc.TAILLE_BLOC, Map.LONGUEUR_GRILLE * Bloc.TAILLE_BLOC);
            float dw = (Bloc.TAILLE_BLOC - w) / 2;
            float dh = (Bloc.TAILLE_BLOC - h) / 2;
            return new Point((int)((resolution.Width - tailleGrille.Width) / 2 + x * Bloc.TAILLE_BLOC - dw), (int)((resolution.Height - tailleGrille.Height) / 2 + y * Bloc.TAILLE_BLOC - dh));
        }

        internal void ajoutEntite(Entite entite)
        {
            if (entite == null)
                return;
            listeEntites.Add(entite);
        }

        internal void supprimerEntite(Entite entite)
        {
            if (entite == null)
                return;
            listeEntites.Remove(entite);
        }
    }
}
