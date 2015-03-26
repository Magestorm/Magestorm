using System;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using SharpHelper;

using Buffer11 = SharpDX.Direct3D11.Buffer;

namespace MageMap
{
    static class Program
    {
        // ReSharper disable AccessToDisposedClosure
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!SharpDevice.IsDirectX11Supported())
            {
                MessageBox.Show("Your graphics card does not support DirectX 11.");
                return;
            }

            RenderForm form = new RenderForm
                              {
                                  Text = "Magestorm Map Editor"
                              };


            using (SharpDevice device = new SharpDevice(form))
            {

                SharpShader shader = new SharpShader(device, "./Content/HLSL.txt",
                                     new SharpShaderDescription
                                         {
                                            VertexShaderFunction = "VS",
                                            PixelShaderFunction = "PS"
                                         },
                                     new []
                                     {  
                                            new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                                            new InputElement("NORMAL", 0, Format.R32G32B32_Float, 12, 0),
                                            new InputElement("TEXCOORD", 0, Format.R32G32_Float, 24, 0)
                                     });

                Buffer11 buffer = shader.CreateBuffer<Matrix>();

                form.KeyDown += (sender, e) =>
                {

                };


                RenderLoop.Run(form, () =>
                {
                    if (device.MustResize)
                    {
                        device.Resize();
                    }

                    device.UpdateAllStates();

                    device.Clear(Color.Black);

                    float ratio = (float)form.ClientRectangle.Width / (float)form.ClientRectangle.Height;

                    Matrix projection = Matrix.PerspectiveFovLH(3.14F / 3.0F, ratio, 1, 1000);

                    Matrix view = Matrix.LookAtLH(new Vector3(0, 10, -40), new Vector3(), Vector3.UnitY);

                    Matrix world = Matrix.RotationY(Environment.TickCount / 1000.0F);

                    Matrix worldViewProjection = world * view * projection;

                    device.UpdateData(buffer, worldViewProjection);

                    world = Matrix.RotationY(Environment.TickCount / 1000.0F) * Matrix.Translation(5, 0, -15);
                    worldViewProjection = world * view * projection;

                    device.UpdateData(buffer, worldViewProjection);

                    world = Matrix.RotationY(Environment.TickCount / 1000.0F) * Matrix.Translation(-5, 0, -15);
                    worldViewProjection = world * view * projection;

                    device.UpdateData(buffer, worldViewProjection);

                    device.Present();
                });

                buffer.Dispose();
            }
        }
    }
}
