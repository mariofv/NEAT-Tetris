using System.Collections;
using System.Collections.Generic;

public class FrameGenerator {

	public static double[] generateFrame(double posX, double posY, int[][] tetrominoShape, int[][] gameboard)
    {
        double[] frame = new double[NEAT.Instance.parameters.NET_INPUTS]; // 280 + 16 + 2

        for (int i = 0; i < 28; ++i)
        {
            for (int j = 0; j < 10; ++j)
            {
                frame[i * 10 + j] = gameboard[i][j];
            }
        }

        for (int i = 0; i < 4; ++i)
        {
            for (int j = 0; j < 4; ++j)
            {
                frame[280 + i * 4 + j] = tetrominoShape[i][j];
            }
        }

        frame[296] = posX;
        frame[297] = posX;

        return frame;

    }

}
