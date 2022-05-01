using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Morabaraba
{
    internal class Player
    {
        //culoare
        //nr piese
        //state
        //randul meu, dupa un timp sa schimb automat randul si sa mut orice?
        //maxim 4 mori
        //avem cazul player vs player sau player vs ia
        public enum PlayerState
        {
            Placing,
            Moving,
            Taking,//pentru moara sa ia piesa adversarului
            Flying
        }
        public bool myTurn { get; set; }
        public bool myColor { get;private set; }//0 alb, 1 negru care are mai mare valoarea la un random ia alb
        public List<BoardCell> myHandCells = new List<BoardCell>();
        public List<BoardCell> myBoardCells = new List<BoardCell>();// variabila de tinut minte o celula pe care am apasat-o pentru miscare
        // cell apasat ( selectata ) = new cell, daca e apasata aceeasi celula de 2 ori sa o deselecteze 
        public PlayerState myState { get; set; }

        public Mill[] myMills = new Mill[4];

        public string name = "";
        public Player(bool myTurn, bool myColor, string name)
        {
            this.myTurn = myTurn;
            this.myColor = myColor;
            myState = PlayerState.Placing;
            this.name = name;
            initializeMills();
            initializeHandCells();
        }

        public void initializeMills()
        {
            for(int i= 0; i < myMills.Length; i++)
            {
                myMills[i] = new Mill();
            }
        }
        public void initializeHandCells()
        {
            for(int i=0;i<12;i++)//12 vaci are fiecare player
            {
                myHandCells.Add(new BoardCell());
            }
        }

        public System.Drawing.Bitmap getMyColor()
        {
            if(myColor == false)
            {
                return Properties.Resources.blackPiece;
            }
            else
            {
                return Properties.Resources.whitePiece;
            }        
        }

        internal void setMyTurn(bool v)
        {
            this.myTurn=v;
        }

        public string getName()
        {
            return this.name;
        }
    }
}
