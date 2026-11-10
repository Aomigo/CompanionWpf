# 🎵 Festival Companion CLI

Festival Companion CLI is a C# console application designed to help manage a multi-day music festival directly from the terminal.

Built as a learning project, it combines object-oriented programming, repository patterns, JSON persistence, LINQ queries, and a modern terminal interface powered by Spectre.Console.

---

## ✨ Features

### 🎤 Artist Management

Create and manage festival artists.

- Register performers
- Store artist information
- Reuse artists when scheduling concerts

### 🎶 Concert Scheduling

Schedule concerts with:

- Artist selection
- Date and time
- Duration
- Stage assignment
- Ticket price

### 🏟️ Stage Organization

Display concerts by stage and by day.

Quickly view:

- Who is performing
- Where they perform
- When they perform
- Concert status

### 📅 Festival Planning

Visualize the entire festival through a hierarchical tree view.

Concerts are automatically categorized as:

- 🟢 Announced
- 🟡 In Progress
- 🔴 Over

Example:

```text
Festival Planning
├── Day 1
│   ├── Scene 1
│   │   ├── Muse at 07:00 PM
│   │   └── Aurora at 09:00 PM
│   └── Scene 2
└── Day 2
```

### 💾 Save & Load

Persist festival data using JSON.

Stored information may include:

- Festival dates
- Artists
- Concerts
- Stage planning

When the application restarts, saved data can be loaded back into memory.

---

## 🏗️ Project Structure

```text
CompanionCLI
│
├── Entities
│   ├── Artist
│   ├── Concert
│   └── FestivalData
│
├── Repositories
│   ├── IConcertRepository
│   └── InMemoryConcertRepository
│
├── Services
│   └── ConcertService
│
└── Program.cs
```

---

## 🧠 Concepts Practiced

This project was built to learn and practice:

- C# and .NET
- Object-Oriented Programming
- Repository Pattern
- Separation of Concerns
- LINQ
- JSON Serialization / Deserialization
- DateTime and DateOnly
- Spectre.Console
- Console Application Design

---

## 🛠️ Technologies

- .NET
- C#
- Spectre.Console
- System.Text.Json
- LINQ

---

## 🚀 Typical Workflow

1. Configure festival dates.
2. Create artists.
3. Schedule concerts.
4. Assign concerts to stages.
5. Display the festival planning.
6. Save data to JSON.
7. Reload the festival later.

---

## 🎯 Purpose

Festival Companion CLI is primarily a learning project focused on building clean C# applications while exploring real-world concepts such as scheduling, persistence, repositories, services, and console-based user interfaces.

---

