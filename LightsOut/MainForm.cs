// Matthew Day

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightsOut {

	public partial class MainForm : Form {

		private const int GridOffset = 25; // Distance from upper-left side of window
		private const int GridBottomOffset = 5; // Distance from buttons at bottom of window
		private int gridLength = 200; // Size in pixels of grid
		private int cellLength;

		private LightsOutGame game;

		public MainForm() {

			InitializeComponent();

			game = new LightsOutGame();

			gridLength = newGameButton.Location.Y - GridOffset - GridBottomOffset;
		}

		private void MainForm_Paint(object sender, PaintEventArgs e) {

			gridLength = newGameButton.Location.Y - GridOffset - GridBottomOffset;

			cellLength = gridLength / game.GridSize;

			Graphics g = e.Graphics;

			for (int r = 0; r < game.GridSize; r++) {

				for (int c = 0; c < game.GridSize; c++) {

					// Get proper pen and brush for on/off
					// grid section
					Brush brush;
					Pen pen;

					if (game.GetGridValue(r, c)) {

						pen = Pens.Black;
						brush = Brushes.White; // On
					} else {

						pen = Pens.White;
						brush = Brushes.Black; // Off
					}

					// Determine (x,y) coord of row and col to draw rectangle
					int x = c * cellLength + GridOffset;
					int y = r * cellLength + GridOffset;

					// Draw outline and inner rectangle
					g.DrawRectangle(pen, x, y, cellLength, cellLength);
					g.FillRectangle(brush, x + 1, y + 1, cellLength - 1, cellLength - 1);
				}
			}
		}

		private void MainForm_MouseDown(object sender, MouseEventArgs e) {

			// Make sure click was inside the grid
			if (e.X < GridOffset || e.X > cellLength * game.GridSize + GridOffset ||
			e.Y < GridOffset || e.Y > cellLength * game.GridSize + GridOffset)
				return;

			// Find row, col of mouse press
			int r = (e.Y - GridOffset) / cellLength;
			int c = (e.X - GridOffset) / cellLength;

			game.Move(r, c);

			// Redraw grid
			this.Invalidate();

			// Check to see if puzzle has been solved
			if (game.IsGameOver()) {

				// Display winner dialog box
				MessageBox.Show(this, "Congratulations! You've won!", "Lights Out!",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void NewGameButton_Click(object sender, EventArgs e) {

			game.NewGame();

			// Redraw grid
			this.Invalidate();
		}

		private void NewToolStripMenuItem_Click(object sender, EventArgs e) {

			NewGameButton_Click(sender, e);
		}

		private void ExitToolStripMenuItem_Click(object sender, EventArgs e) {

			Close();
		}

		private void ExitButton_Click(object sender, EventArgs e) {

			ExitToolStripMenuItem_Click(sender, e);
		}

		private void AboutToolStripMenuItem_Click(object sender, EventArgs e) {

			AboutForm aboutBox = new AboutForm();
			aboutBox.ShowDialog(this);
		}

		private void MainForm_Resize(object sender, EventArgs e) {

			// Redraw grid
			this.Invalidate();
		}

		private void X3ToolStripMenuItem_Click(object sender, EventArgs e) {

			// Set checked states for menu items
			x3ToolStripMenuItem.Checked = true;
			x4ToolStripMenuItem.Checked = false;
			x5ToolStripMenuItem.Checked = false;

			game.GridSize = 3;

			// Redraw grid
			this.Invalidate();
		}

		private void X4ToolStripMenuItem_Click(object sender, EventArgs e) {

			// Set checked states for menu items
			x3ToolStripMenuItem.Checked = false;
			x4ToolStripMenuItem.Checked = true;
			x5ToolStripMenuItem.Checked = false;

			game.GridSize = 4;

			// Redraw grid
			this.Invalidate();
		}

		private void X5ToolStripMenuItem_Click(object sender, EventArgs e) {

			// Set checked states for menu items
			x3ToolStripMenuItem.Checked = false;
			x4ToolStripMenuItem.Checked = false;
			x5ToolStripMenuItem.Checked = true;

			game.GridSize = 5;

			// Redraw grid
			this.Invalidate();
		}
	}
}
