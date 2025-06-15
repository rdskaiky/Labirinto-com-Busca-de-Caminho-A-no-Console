
using System;
using System.Collections.Generic;

class No
{
    public int X, Y;
    public int G, H;
    public No Pai;

    public int F => G + H;

    public No(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override bool Equals(object obj)
    {
        if (obj is No outro)
            return X == outro.X && Y == outro.Y;
        return false;
    }

    public override int GetHashCode() => X * 1000 + Y;
}

class Labirinto
{
    static int largura = 20;
    static int altura = 10;
    static char[,] mapa = new char[altura, largura];
    static int[,] direcoes = new int[,] { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };
    static No inicio = new No(1, 1);
    static No fim = new No(8, 18);

    static void Main()
    {
        GerarLabirinto();
        List<No> caminho = EncontrarCaminhoAStar();

        foreach (var no in caminho)
        {
            if (!(no.X == inicio.X && no.Y == inicio.Y) && !(no.X == fim.X && no.Y == fim.Y))
                mapa[no.X, no.Y] = '*';
        }

        MostrarLabirinto();
    }

    static void GerarLabirinto()
    {
        Random rnd = new Random();
        for (int i = 0; i < altura; i++)
            for (int j = 0; j < largura; j++)
                mapa[i, j] = (rnd.NextDouble() < 0.2) ? '#' : ' ';

        mapa[inicio.X, inicio.Y] = 'S';
        mapa[fim.X, fim.Y] = 'E';
    }

    static List<No> EncontrarCaminhoAStar()
    {
        var abertos = new List<No>();
        var fechados = new HashSet<No>();

        inicio.G = 0;
        inicio.H = Heuristica(inicio, fim);
        abertos.Add(inicio);

        while (abertos.Count > 0)
        {
            abertos.Sort((a, b) => a.F.CompareTo(b.F));
            No atual = abertos[0];
            abertos.RemoveAt(0);

            if (atual.Equals(fim))
                return ReconstruirCaminho(atual);

            fechados.Add(atual);

            for (int i = 0; i < 4; i++)
            {
                int novoX = atual.X + direcoes[i, 0];
                int novoY = atual.Y + direcoes[i, 1];

                if (!EhValido(novoX, novoY)) continue;

                No vizinho = new No(novoX, novoY);

                if (fechados.Contains(vizinho)) continue;

                int gTemp = atual.G + 1;

                bool noAberto = abertos.Contains(vizinho);
                if (!noAberto || gTemp < vizinho.G)
                {
                    vizinho.G = gTemp;
                    vizinho.H = Heuristica(vizinho, fim);
                    vizinho.Pai = atual;

                    if (!noAberto)
                        abertos.Add(vizinho);
                }
            }
        }

        return new List<No>();
    }

    static int Heuristica(No a, No b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }

    static List<No> ReconstruirCaminho(No no)
    {
        List<No> caminho = new List<No>();
        while (no != null)
        {
            caminho.Add(no);
            no = no.Pai;
        }
        caminho.Reverse();
        return caminho;
    }

    static bool EhValido(int x, int y)
    {
        return x >= 0 && x < altura && y >= 0 && y < largura && mapa[x, y] != '#';
    }

    static void MostrarLabirinto()
    {
        for (int i = 0; i < altura; i++)
        {
            for (int j = 0; j < largura; j++)
                Console.Write(mapa[i, j]);
            Console.WriteLine();
        }
    }
}
