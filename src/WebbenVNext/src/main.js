class PreviewImage {
    constructor(url) {
        this.url = url;
    }
    toHtml() {
        return `<div class="card">
                    <img src="${this.url}" class="img-fluid" />
                </div>`;
    }
}
class FileApplication {
    constructor() {
        this.filesInput = document.querySelector('#files');
        this.filesPreviewWrapper = document.querySelector('#files-preview');
    }
    run() {
        if (this.filesInput !== undefined && this.filesPreviewWrapper !== undefined) {
            this.filesInput.addEventListener('change', event => {
                this.previewImages();
            });
        }
    }
    previewImages() {
        this.filesPreviewWrapper.innerHTML = '';
        let files = this.filesInput.files;
        for (let i = 0; i < files.length; i++) {
            this.previewImage(files[0]);
        }
    }
    previewImage(file) {
        let fileReader = new FileReader();
        fileReader.addEventListener('load', () => {
            let image = new PreviewImage(fileReader.result);
            this.filesPreviewWrapper.innerHTML += image.toHtml();
        });
        fileReader.readAsDataURL(file);
    }
}
var app = new FileApplication();
app.run();
//# sourceMappingURL=main.js.map