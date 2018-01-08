using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.PointOfService;

namespace FirstStep
{
	/// <summary>
	/// Description of frmStep1.
	/// </summary>
	public class FrameFirstStep : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnExample1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnExample2;
		/// <summary>
		/// Design  variable. 
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FrameFirstStep()
		{
			//
			// The InitializeComponent() call is required for windows Forms designer support.
			//
			InitializeComponent();

			//
			// TODO: Add counstructor code after the InitializeComponent() call.
			//
		}

		/// <summary>
		/// Rear treatment is carried out in the resource being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Forms Designer generated code.
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. 
		/// The Forms designer might not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnExample1 = new System.Windows.Forms.Button();
			this.btnExample2 = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnExample1
			// 
			this.btnExample1.Location = new System.Drawing.Point(88, 44);
			this.btnExample1.Name = "btnExample1";
			this.btnExample1.Size = new System.Drawing.Size(112, 24);
			this.btnExample1.TabIndex = 0;
			this.btnExample1.Text = "Example1";
			this.btnExample1.Click += new System.EventHandler(this.btnExample1_Click);
			// 
			// btnExample2
			// 
			this.btnExample2.Location = new System.Drawing.Point(40, 72);
			this.btnExample2.Name = "btnExample2";
			this.btnExample2.Size = new System.Drawing.Size(112, 24);
			this.btnExample2.TabIndex = 2;
			this.btnExample2.Text = "Example2";
			this.btnExample2.Click += new System.EventHandler(this.btnExample2_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnExample2);
			this.groupBox1.Location = new System.Drawing.Point(48, 20);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(188, 120);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Exsample";
			// 
			// FrameFirstStep
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 12);
			this.ClientSize = new System.Drawing.Size(292, 149);
			this.Controls.Add(this.btnExample1);
			this.Controls.Add(this.groupBox1);
			this.MaximizeBox = false;
			this.Name = "FrameFirstStep";
			this.Text = "First Step \"How to use PosExplorer\"";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Main entry point.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new FrameFirstStep());
		}

		
		/// <summary>
		/// PosCommon object
		/// </summary>
		PosCommon m_PosCommon = null;

		/// <summary>
		/// Logical Name
		/// </summary>
		string m_strLogicalName = "PosPrinter";

		/// <summary>
		/// How to use "PosExplorer" sample1.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnExample1_Click(object sender, System.EventArgs e)
		{
			//Use a Logical Device Name which has been set on the SetupPOS.
			try
			{
				//Create PosExplorer
				PosExplorer posExplorer = new PosExplorer();
			
				DeviceInfo deviceInfo = null;

				//Get DeviceInfo use devicecategory and logicalname.
				deviceInfo = posExplorer.GetDevice(DeviceType.PosPrinter,m_strLogicalName);
			
				m_PosCommon =(PosCommon)posExplorer.CreateInstance(deviceInfo);
			
				//Open the device
				m_PosCommon.Open();

				//Get the exclusive control right for the opened device.
				//Then the device is disable from other application.
				m_PosCommon.Claim(1000);

				//Enable the device.
				m_PosCommon.DeviceEnabled = true;

				//CheckHealth.
				m_PosCommon.CheckHealth(Microsoft.PointOfService.HealthCheckLevel.Interactive);

				//Close device
				m_PosCommon.Close();

			}
			catch(Exception)
			{
			}
		}

		/// <summary>
		/// How to use "PosExplorer" sample2.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnExample2_Click(object sender, System.EventArgs e)
		{
			
			try
			{
				//Create PosExplorer
				PosExplorer posExplorer = new PosExplorer();
				DeviceInfo deviceInfo = null;
				DeviceCollection deviceCollection;

				//Get DeviceCollection use device categoly
				deviceCollection = posExplorer.GetDevices(DeviceType.PosPrinter);
				string[] astrLogicalNames;

				Hashtable hashDevice = new Hashtable(0);
				foreach( DeviceInfo devInfo in deviceCollection )
				{

					//if the device name is registered. 
					if(devInfo.LogicalNames.Length > 0)
					{
						astrLogicalNames = devInfo.LogicalNames;
						for(int i = 0; i < astrLogicalNames.Length;i++)
						{
							if(!hashDevice.ContainsKey(astrLogicalNames[i]))
							{
								m_PosCommon =(PosCommon)posExplorer.CreateInstance(devInfo);

								//Use Legacy Opos
								//if(m_PosCommon.Compatibility.Equals(DeviceCompatibilities.Opos))
								//Use Opos for .Net
								if(m_PosCommon.Compatibility.Equals
									(DeviceCompatibilities.CompatibilityLevel1))
								{
									try
									{
										//Register hashtable key:LogicalName,Value:DeviceInfo
										hashDevice.Add(astrLogicalNames[i],devInfo);
									}
									catch(Exception)
									{
									}
								}
								
							}
						}		
					}
				}

				deviceInfo = (DeviceInfo)hashDevice[m_strLogicalName];

				m_PosCommon =(PosCommon)posExplorer.CreateInstance(deviceInfo);

				//Open the device
				m_PosCommon.Open();

				//Get the exclusive control right for the opened device.
				//Then the device is disable from other application.
				m_PosCommon.Claim(1000);

				//Enable the device.
				m_PosCommon.DeviceEnabled = true;

				//CheckHealth.
				m_PosCommon.CheckHealth(Microsoft.PointOfService.HealthCheckLevel.Interactive);

				//Close device
				m_PosCommon.Close();

			}
			catch(Exception)
			{
			}
		}

		/// <summary>
		/// When the method "closing" is called,
		/// the following code is run.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			try
			{
				if(m_PosCommon != null)
				{
					//Cancel the device
					m_PosCommon.DeviceEnabled = false;

					//Release the device exclusive control right.
					m_PosCommon.Release();

					//Finish using the device.
					m_PosCommon.Close();
				}
			}
			catch(PosControlException)
			{
			}
		}

		
	}
}
