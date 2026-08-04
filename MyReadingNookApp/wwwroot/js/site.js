// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
    $(document).ready(function () {
    let searchTimeout;

    $("#searchInput").on("input", function () {
    clearTimeout(searchTimeout);
    const query = $(this).val().trim();

    if (query.length === 0) {
    $("#searchResults").empty();
    return;
}

    searchTimeout = setTimeout(function () {
    $.ajax({
    url: "/api/Books/search",
    method: "GET",
    data: { title: query },
    success: function (response) {
    $("#searchResults").empty();

    response.forEach(function (book) {
    const imagePath = book.thumbnail || "https://via.placeholder.com/40x60?text=No+Image";

    const resultHtml = `
                                            <div class="search-result-item" data-isbn="${book.isbn}">
                                                <img src="${imagePath}" alt="${book.title}">
                                                <div>
                                                    <div><strong>${book.title}</strong></div>
                                                    <div class="text-muted" style="font-size: 0.85em;">${book.author}</div>
                                                </div>
                                            </div>`;
    $("#searchResults").append(resultHtml);
});
},
    error: function (xhr) {
    $("#searchResults").empty();
}
});
}, 400);
});

    $(document).on("click", function (event) {
    if (!$(event.target).closest(".search-container, #searchResults").length) {
    $("#searchResults").empty();
}
});

    $(document).on("click", ".search-result-item", function (event) {
    const isbn = $(this).data("isbn");
    window.location.href = "/BookData/Details?isbn=" + encodeURIComponent(isbn);
});
});
