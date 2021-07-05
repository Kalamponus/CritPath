
namespace Dialog
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.buttonMakeSolutionAndSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // buttonDownload
            // 
            this.buttonDownload.Location = new System.Drawing.Point(29, 94);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(191, 39);
            this.buttonDownload.TabIndex = 0;
            this.buttonDownload.Text = "Загрузить";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // buttonMakeSolutionAndSave
            // 
            this.buttonMakeSolutionAndSave.Location = new System.Drawing.Point(29, 184);
            this.buttonMakeSolutionAndSave.Name = "buttonMakeSolutionAndSave";
            this.buttonMakeSolutionAndSave.Size = new System.Drawing.Size(191, 46);
            this.buttonMakeSolutionAndSave.TabIndex = 0;
            this.buttonMakeSolutionAndSave.Text = "Получить и сохранить решение";
            this.buttonMakeSolutionAndSave.UseVisualStyleBackColor = true;
            this.buttonMakeSolutionAndSave.Click += new System.EventHandler(this.buttonMakeSolutionAndSave_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 316);
            this.Controls.Add(this.buttonMakeSolutionAndSave);
            this.Controls.Add(this.buttonDownload);
            this.Name = "Form1";
            this.Text = "Приветствую";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.Button buttonMakeSolutionAndSave;
    }
}

