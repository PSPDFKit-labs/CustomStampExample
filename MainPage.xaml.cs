using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using PSPDFKit.Document;
using PSPDFKit.Pdf;
using PSPDFKit.UI;
using System.Threading.Tasks;

using PSPDFKit.UI.ToolbarComponents;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CustomStampExample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            SetCustomStamp();
            
            // This handler is invoked once the `PdfView` has been initialized, and then the `PdfView.Controller` can be used.
            PdfView.InitializationCompletedHandler += delegate (PdfView sender, Document args)
            {
                // Now that the `PdfView` is ready, enable the button for opening a file.
                Button_OpenPDF.IsEnabled = true;

                
            };
        }

        private async void PdfView_InitializationCompletedHandlerAsync(PdfView sender, Document args)
        {
            await sender.Controller.SetKeepSelectedToolAsync(true);
        }

        private async void Button_OpenPDF_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".pdf");

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var document = DocumentSource.CreateFromStorageFile(file);
                await PdfView.Controller.ShowDocumentAsync(document);
            }
        }
        
        private async Task SetCustomStamp()
        {
            var toolbarItemsList = PdfView.GetToolbarItems();

            IEnumerable<IToolbarItem> toolbarsItems = toolbarItemsList.Where(t => t is StampToolbarItem || t is ImageToolbarItem);
            foreach (IToolbarItem toolbarItem in toolbarsItems)
            {
                toolbarItem.Attributes.DropdownGroup = "Images/Tampons";
                toolbarItem.Attributes.ResponsiveGroup = "my-group";
            }

            string customStampsId = "customStampsButton";
            ButtonToolbarItem customStampButton = new ButtonToolbarItem()
            {
                Attributes =
                {
                    Id = customStampsId,
                    Title = "Tampons personnalisés",
                    DropdownGroup = "Images/Tampons",
                    ResponsiveGroup = "my-group",
                    
                },
                Disabled = false,
                Icon = new Uri("ms-appx-web:///Assets/paste.svg")
            };
            customStampButton.OnItemPressEvent += OpenCustomStampMenuHandler;
            toolbarItemsList.Add(customStampButton);
            
            await PdfView.SetToolbarItemsAsync(toolbarItemsList);
        }

        private void OpenCustomStampMenuHandler(IToolbarItem sender, string args)
        {
        }
    }
}
