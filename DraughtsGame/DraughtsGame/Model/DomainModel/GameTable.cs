using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DraughtsGame.Model.DomainModel
{
    
    public class GameTable
    {
        private const int NUMBER_OF_LINES = 8;
        private const int NUMBER_OF_COLUMNS = 8; 

        private  GameUtil.CellState[,] gameTable;

        public GameTable()
        {
            InitializeGameTable();
        }

        private void InitializeGameTable()
        {
            gameTable = new GameUtil.CellState[NUMBER_OF_LINES, NUMBER_OF_COLUMNS];

            for (int i = 0; i < NUMBER_OF_LINES; i++)
            {
                for (int j = 0; j < NUMBER_OF_COLUMNS; j++)
                {
                    gameTable[i, j] = GameUtil.CellState.UNITIALIZED;
                }
            }

            InitializeBlackQueen();
            InitializeRedQueen();
        }

        private void InitializeBlackQueen()
        {
            for(int i = 0; i < 3; i++)
            {
                for(int j=((i+1)%2==0)?1:0;j < NUMBER_OF_COLUMNS; j+=2)
                {
                    gameTable[i, j] = GameUtil.CellState.BLACK;
                } 
            }
        }

        private void InitializeRedQueen()
        {
            for (int i = 5; i < NUMBER_OF_LINES; i++)
            {
                for (int j = ((i + 1) % 2 == 0) ? 0 : 1; j < NUMBER_OF_COLUMNS; j += 2)
                {
                    gameTable[i, j] = GameUtil.CellState.RED;
                }
            }
        }

        public void GetCellStateAtPosition(int row,int column, GameUtil.CellState cellState)
        {
            gameTable[row, column] = cellState;
        }

        public GameUtil.CellState GetCellStateAtPosition(int row,int column) 
        {
            return gameTable[row, column];
        }

        public void RestartGameTable()
        {
            InitializeGameTable();
        }

        public int GetNumberOfLines()
        {
            return NUMBER_OF_LINES;
        }

        public int GetNumberOfColumn()
        {
            return NUMBER_OF_COLUMNS;
        }

    }
}
