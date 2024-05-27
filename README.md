Kanban Board V.2.. 
Tapi sebelumnya, Apa itu Kanban Board?
Kanban adalah cara untuk membantu tim menyeimbangkan pekerjaan yang harus dilakukan dengan kapasitas yang tersedia dari setiap anggota tim.

Tapi, Tujuannya dibuat aplikasi ini apa?
Yaitu untuk bisa mengorganisir kehidupan dengan adanya lightweight CLI App yang bisa dengan cepat merubah antar profil atau pengguna untuk papan khusus dan task-task khusus.
Apa dependencies yang digunakan di aplikasi ini?
1. Mysql.EntityFrameworkCore
2. Newtonsoft.Json


Sebagai perkembangan dari versi ketika presentasi kuis, ada beberapa hal yang dikembangkan:
1. Fitur login dan registrasi
2. Pembuatan papan
3. Pembuatan task di masing-masing papan dan opsi masing-masing task.

RENAME
Akan diminta id yang tersedia dari task keseluruhan tabel entitas, kemudian input nama baru agar diupdate di tabel.

MOVE
Akan diminta id yang tersedia dari task keseluruhan tabel, khusus untuk pengguna tersebut, dan khusus untuk masing-masing tabel, kemudian diminta untuk memindahkan ke kolom mana (To-do, Doing, atau Done)

DELETE
Pengguna akan diminta id nya task yang mana untuk dihapuskan. Akan memunculkan teks “Task successfully deleted!”
