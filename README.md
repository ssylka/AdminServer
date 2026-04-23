# 🧩 User Management Web Application

## 📌 Overview

This project is a **web application with user registration and management**, built using the required C#/.NET stack.

The focus is on:

* Proper database design
* Clean UI (table + toolbar)
* Access control & user state management

---

## ⚙️ Tech Stack Requirements

* **C# / ASP.NET**
* SQL Server / MySQL / PostgreSQL
* Any CSS framework (**Bootstrap recommended**)

---

## 🚨 Core Requirements

### 🗄️ Database

* Must implement a **UNIQUE INDEX** (mandatory)
* Email uniqueness must be enforced at the database level
* Unique index ≠ primary key (both should exist)

---

### 🔐 Authentication & Access

* Users must:

  * Register
  * Log in

* Access rules:

  * ❌ Non-authenticated users:

    * Only login/registration allowed
  * ❌ Blocked users:

    * Cannot log in
  * ✅ Authenticated & active users:

    * Full access to user management

---

### 📧 Email Verification

* After registration:

  * User is redirected
  * Confirmation email is sent **asynchronously**
* Clicking the link:

  * Changes status from `unverified` → `active`
* Blocked users remain blocked regardless

---

### 👤 User States

Each user has a status:

* `Active`
* `Unverified`
* `Blocked`

Additional data:

* Name
* Email
* Last login time
* Registration time (optional)

---

### 🧠 Business Rules

* Any user can:

  * Block users
  * Delete users (including themselves)
* Deleted users:

  * Must be **fully removed** (not soft delete)
  * Can re-register
* Password:

  * Any non-empty value is valid

---

## 📊 UI Requirements

### 📋 Table

* Displays all users
* Must support:

  * Sorting (e.g., by last login time)
  * Multiple selection (checkboxes)

#### Selection Rules:

* Checkbox column has **no label**
* Header checkbox selects/deselects all rows

---

### 🧰 Toolbar

Located above the table, includes actions:

* Block (with text)
* Unblock (icon)
* Delete (icon)
* Delete unverified (icon)

⚠️ Important:

* No buttons inside table rows
* Buttons should enable/disable based on selection

---

### 🎨 UI Constraints

* Must look like:

  * Table + Toolbar layout

* No:

  * Animations
  * Wallpapers under table
  * Browser alerts

* Should be:

  * Clean
  * Business-style
  * Responsive (desktop + mobile)

---

## 🔄 Behavior Requirements

### 🔁 Request Validation

Before any request (except login/register):

* Server must check:

  * User exists
  * User is not blocked

Otherwise:

* Redirect to login page

---

### 📈 Sorting

* Data must be sortable
* Example: by last login time

---

### 📬 Feedback & UX

Must include:

* Error messages
* Tooltips
* Status messages (success/failure)

---

## 🏗️ Implementation Notes

### 📦 Libraries

* Use existing libraries where possible
* Do NOT reinvent standard solutions

---

### 📧 Email Sending

* Any method allowed (e.g., Gmail SMTP)
* Test solutions are acceptable

---

### 🧪 Additional Notes

* Application must be:

  * Fully working
  * Deployed (remotely accessible)

* UI should resemble:

  * A professional admin panel
  * Simple but consistent design

---

## ⚠️ Important Constraints

* No buttons inside table rows
* No alert popups
* Must use checkboxes + toolbar
* Must use CSS framework

---

## 🎯 Summary

This task evaluates:

* Backend correctness (DB + logic)
* Proper use of indexes
* UI/UX discipline
* Real-world admin panel behavior
