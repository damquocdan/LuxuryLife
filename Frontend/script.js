// URL của API
const apiUrl = "https://localhost:7182/api/Listimagestours";

// Hàm gọi API và hiển thị dữ liệu
async function fetchAndDisplayData() {
  try {
    // Gọi API
    const response = await fetch(apiUrl);
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    
    // Chuyển đổi dữ liệu JSON
    const data = await response.json();
    
    // Lấy phần tbody của bảng
    const tableBody = document.querySelector("#tourTable tbody");
    
    // Xóa nội dung cũ (nếu có)
    tableBody.innerHTML = "";

    // Lặp qua danh sách và thêm dữ liệu vào bảng
    data.forEach(item => {
      const row = document.createElement("tr");

      row.innerHTML = `
        <td>${item.listimagestourId}</td>
        <td>${item.tourId}</td>
        <td><img src="${item.imageUrl}" alt="Tour Image"></td>
        <td>${item.imageDescription}</td>
        <td>${new Date(item.createdate).toLocaleDateString()}</td>
      `;

      tableBody.appendChild(row);
    });
  } catch (error) {
    console.error("Error fetching data:", error);
  }
}

// Gọi hàm để lấy và hiển thị dữ liệu
fetchAndDisplayData();
