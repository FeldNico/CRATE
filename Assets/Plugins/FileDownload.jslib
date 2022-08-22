var ZipDownloaderPlugin = {
  ZipDownloader: function(base64, fn) {
      var fname = Pointer_stringify(fn);
      var link = document.createElement('a');
    link.download = fname;
    link.innerHTML = 'DownloadFile';
    link.setAttribute('id', 'ZipDownloaderLink');
    link.href = 'data:application/zip;base64,' + Pointer_stringify(base64);
            link.onclick = function()
            {
                var child = document.getElementById('ZipDownloaderLink');
                child.parentNode.removeChild(child);
            };
            link.style.display = 'none';
            document.body.appendChild(link);
    link.click();
  }
};
mergeInto(LibraryManager.library, ZipDownloaderPlugin);