- Khi em khởi chạy thử project gốc, em thấy có lỗi do thiếu Button Timer nên em đã sửa lại UI và thêm button Timer vào.

Task 1: Re-skin
- Với mỗi prefab, em sẽ thay sprite cũ thành các sprite fish bằng cách kéo các texture fish vào spite trong sprite renderer của mỗi prefab

Task 2: Change Gameplay
- Đầu tiên em sẽ bỏ các cơ chế swap trước đó ở trong BoardController, và thêm cơ chế tapping để đưa item vào Bottom Cell.
- Tạo thêm 1 class bottomArea để quản lý 5 bottom cell, bottom cell có thể check là nó trống hay không, và bottomArea sẽ biết được còn bao nhiêu bottom cell trống và khi nào sẽ full. Khi add vào bottom cell, em sẽ thử add vào và đồng thời check xem trong dãy bottom cell đó có 3 item nào giống nhau hay không. Trong bottom area em sẽ phát 2 tín hiệu là khi full và khi thấy 3 item giống nhau
- khi add vào thành công và có 3 item giống cạnh nhau thì em sẽ cho đợi khoảng 0.3s đề explode kết thúc, nếu không vẫn add vào và ko có j xảy ra. Tiếp theo em sẽ đăng ký lắng nghe sự kiện full bottom cell hay board hết để kiểm tra game win hay lose. Khi Win hay Lose, em sẽ phát ra tín hiệu Win hoặc Lose để khi mà win nó sẽ hiện ra scene win, và ngược lại.
- Với yêu cầu số lượng của mỗi item chia hết cho 3: đầu tiên em sẽ tạo danh sách item mỗi loại chia đều, sau đó nếu còn cell trống e sẽ chọn loại ngẫu nhiên và thêm 3 item cùng loại đó.

Task 3: Vì trong vòng 4 tiếng, em không thể hoàn thiện hết task 3. Nhưng em sẽ cố gắng hoàn thiện nó trong thời gian sớm nhất
