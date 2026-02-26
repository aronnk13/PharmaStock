# PharmaStock Contribution Guidelines

Welcome to the **PharmaStock Capstone Project**!  
To keep our codebase clean, professional, and conflict-free, everyone must follow these rules before writing code or raising a Pull Request (PR).

---

## 1. Branch Naming Conventions
- **Never push directly to the `main` branch.**
- Always create a new branch from `main` before starting your work.

**Branch Types:**
- **Features:** `feat/your-name/short-description`  
  Example: `feat/arun/add-location-model`
- **Bug Fixes:** `fix/your-name/short-description`  
  Example: `fix/aditya/fix-login-crash`

---

## 2. Commit Message Rules
- Keep commit messages **clear and descriptive**.
- The message should explain what you did without needing to read the code.

**Examples:**
- Good: `Added Location and Bin models with EF Core relations`
- Bad: `fixed stuff`, `updated code`, `wip`

---

## 3. Pull Request (PR) Rules

### Naming Your PR
- Your PR title should clearly state what it does.
- Use tags at the beginning:
  - `[Feature] Added 10 Inventory Tables`
  - `[Fix] Corrected FK in TransferOrder`

### When Can You Merge?
- You **cannot merge your own PR**.
- Open the PR and assign at least one other team member (or the Team Lead) as a **Reviewer**.
- The code must **build successfully locally with zero errors**.
- If there are merge conflicts, the **PR raiser** is responsible for fixing them before requesting a review.
- Only merge after getting **at least 1 approval**.

---

## 4. C# Naming Conventions
We strictly follow **Microsoft C# naming conventions**:

| Element              | Convention | Example                          |
|----------------------|------------|----------------------------------|
| Classes & Interfaces | PascalCase | `InventoryBalance`, `IAdminRepo` |
| Methods/Functions    | PascalCase | `GetStockByLocation()`           |
| Public Properties    | PascalCase | `public string DrugID { get; set; }` |
| Local Variables & Parameters | camelCase | `int orderedQty`, `string itemName` |
| Private Fields       | camelCase with `_` | `_dbContext`, `_repository` |

---

## 5. Commenting Etiquette
- Code should mostly explain itself through **good naming**.
- When comments are needed, follow these strict rules:
  - **NO AI-generated filler:** Do not paste ChatGPT/Copilot explanations verbatim.
  - **NO Emojis or slang:** Keep it strictly professional.
  - **Focus on the "Why":**  
    - Bad: `// Adds 1 to count`  
    - Good: `// Adding 1 to account for the quarantine bin offset`
- Delete all commented-out **dead code** before raising a PR.  
  - If we need the old code back, we will find it in **Git history**.

---
