// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const isLoginPage = window.location.pathname.includes("/Account/Login")
// Check if auth token exists
if (!sessionStorage.getItem('authToken') && !isLoginPage) {
  // Redirect to login page
  window.location.href = "/Account/Login";
}

$.ajaxSetup({
  statusCode: {
    401: function () {
      if (isLoginPage == false) {
        window.location.href = "/Account/Login";
      }
    }
  }
})

$(document).ready(function () {
  $.ajax({
    url: "/api/User/username",
    method: "GET",
    contentType: "application/json",
    headers: {
      Authorization: "Bearer " + sessionStorage.getItem('authToken'),
    },
    success: function (response) {
      $("#usernameDisplay").html(response);
    },
    error: function (xhr) {
      $("#usernameDisplay").html("Not Found");
    },
  });

  $.ajax({
    url: "/api/User/current",
    method: "GET",
    headers: {
      Authorization: "Bearer " + sessionStorage.getItem("authToken")
    },
    success: function (response) {
      $("#currentReadingLoading").addClass("d-none");
      $("#currentBooksBadge").text(response.currentBooksCount + (response.currentBooksCount === 1 ? " Active" : " Active"));

      if (!response.books || response.books.length === 0) {
        $("#currentReadingEmpty").removeClass("d-none");
        return;
      }

      const inner = $("#carouselInner");
      response.books.forEach(function (book, index) {
        const cover = book.imagePath || "https://via.placeholder.com/140x210?text=No+Cover";
        const activeClass = index === 0 ? "active" : "";

        const slideHtml = `
                        <div class="carousel-item ${activeClass}">
                            <a href="/BookData/Details?bookId=${book.bookId}" class="text-decoration-none">
                                <div class="book-cover-frame mx-auto mb-3 shadow" style="width: 140px; height: 210px; background-color: #e0d5b9; border: 1px solid #c7b790; border-radius: 8px; padding: 6px;">
                                    <img src="${cover}" alt="${book.title}" class="w-100 h-100 rounded" style="object-fit: cover;">
                                </div>
                            </a>
                            <h4 class="fw-bold mb-1 text-truncate" style="color: #4d4835;">${book.title}</h5>
                            <p class="fs-5 mb-0" style="color: #8a8168;">${book.author}</p>
                        </div>
                    `;
        inner.append(slideHtml);
      });

      $("#currentlyReadingCarousel").removeClass("d-none");
    },
    error: function (xhr) {
      $("#currentReadingLoading").addClass("d-none");
      $("#currentReadingEmpty").removeClass("d-none");
    }
  });

  $("#bellBtn").on("click", function (event) {
    window.location.href = "/FriendView/Invites";
  })

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
        headers: {
          Authorization: "Bearer " + sessionStorage.getItem("authToken")
        },
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
  $("#logoutBtn").on("click", function (e) {
    e.preventDefault();

    sessionStorage.removeItem("authToken");

    window.location.href = "/Account/Login";
  })
});
