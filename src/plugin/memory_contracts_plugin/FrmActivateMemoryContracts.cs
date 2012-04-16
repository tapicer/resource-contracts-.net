using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DCFCENUBA.memory_contracts_plugin
{
    public partial class FrmActivateMemoryContracts : Form
    {
        EnvDTE.Projects projects;
        string contractsAssemblyPath;

        public FrmActivateMemoryContracts(EnvDTE.Projects projects, string contractsAssemblyPath)
        {
            InitializeComponent();

            this.projects = projects;
            this.contractsAssemblyPath = contractsAssemblyPath;
        }

        class ProjectData
        {
            public string Name { get; set; }
            public string MemoryContractsEnabled { get; set; }
            public bool EnableDisable { get; set; }
        }

        private void FrmActivateMemoryContracts_Load(object sender, EventArgs e)
        {
            List<ProjectData> projectsData = new List<ProjectData>();

            for (int i = 1; i <= projects.Count; i++)
            {
                var proj = projects.Item(i).Object as VSLangProj.VSProject;

                var found = false;
                for (int j = 1; j < proj.References.Count; j++)
                {
                    if (proj.References.Item(j).Path == contractsAssemblyPath || proj.References.Item(j).Name == "Contracts")
                    {
                        found = true;
                        break;
                    }
                }

                projectsData.Add(new ProjectData()
                    {
                        Name = proj.Project.Name,
                        MemoryContractsEnabled = found ? "Yes" : "No",
                        EnableDisable = found
                    });
            }

            dtgProjects.DataSource = projectsData;
            dtgProjects.Columns[0].HeaderText = "Project Name";
            dtgProjects.Columns[0].Width = 100;
            dtgProjects.Columns[0].ReadOnly = true;
            dtgProjects.Columns[1].HeaderText = "Memory Contracts Enabled";
            dtgProjects.Columns[1].Width = 200;
            dtgProjects.Columns[1].ReadOnly = true;
            dtgProjects.Columns[2].HeaderText = "Enable/Disable";
            dtgProjects.Columns[2].Width = 100;
            dtgProjects.CurrentCellDirtyStateChanged += delegate(object innerSender, EventArgs innerE)
                {
                    dtgProjects.CommitEdit(DataGridViewDataErrorContexts.Commit);
                };
            dtgProjects.CellValueChanged += delegate(object innerSender, DataGridViewCellEventArgs innerE)
                {
                    var currentCell = innerE;
                    if (currentCell.ColumnIndex == 2)
                    {
                        VSLangProj.VSProject prj = null;

                        for (int i = 1; i <= projects.Count; i++)
                        {
                            if (projects.Item(i).Name == dtgProjects[0, currentCell.RowIndex].Value.ToString())
                            {
                                prj = projects.Item(i).Object as VSLangProj.VSProject;
                            }
                        }

                        if (prj != null)
                        {
                            if (dtgProjects[1, currentCell.RowIndex].Value.ToString() == "Yes")
                            {
                                dtgProjects[1, currentCell.RowIndex].Value = "No";
                                for (int j = 1; j < prj.References.Count; j++)
                                {
                                    if (prj.References.Item(j).Path == contractsAssemblyPath || prj.References.Item(j).Name == "Contracts")
                                    {
                                        prj.References.Item(j).Remove();
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                dtgProjects[1, currentCell.RowIndex].Value = "Yes";
                                prj.References.Add(contractsAssemblyPath);
                                MessageBox.Show(string.Format("Memory Contracts are now enabled for the project {0}, remember to enable Code Contracts static checking for the project.", dtgProjects[0, currentCell.RowIndex].Value));
                            }
                        }
                    }
                };
        }

        private void FrmActivateMemoryContracts_Resize(object sender, EventArgs e)
        {
            dtgProjects.Width = this.Width - 40;
            dtgProjects.Height = this.Height - 60;
        }
    }
}
