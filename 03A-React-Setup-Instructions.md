# Step 3A: React Frontend Setup - User Management UI
## Instructions for GitHub Copilot

---

## Overview
This document provides instructions for setting up a **React frontend** project using Vite, TypeScript, TailwindCSS, and integrating it into your Visual Studio solution. This will be the user interface for your Inventory Management System.

**Important**: This file contains instructions for setting up the React project and installing all necessary dependencies.

---

## Technology Stack for Frontend

### Core Technologies:
- **React 18** - Modern UI library with hooks
- **TypeScript** - Type safety and better developer experience
- **Vite** - Fast build tool (much faster than Create React App)

### Styling:
- **TailwindCSS** - Utility-first CSS framework for beautiful, responsive UI
- **Headless UI** - Accessible UI components (works great with Tailwind)
- **Heroicons** - Beautiful icons from Tailwind team

### Routing & Navigation:
- **React Router v6** - Client-side routing

### API Communication:
- **Axios** - HTTP client for API calls
- **Axios Interceptors** - For JWT token handling

### State Management:
- **React Context API** - For global state (auth, user info)
- **Custom Hooks** - For reusable logic

### Testing:
- **Vitest** - Fast unit test runner (Vite's native test runner)
- **React Testing Library** - Testing React components
- **@testing-library/jest-dom** - Custom matchers

### Form Handling:
- **React Hook Form** - Performant form validation
- **Zod** - TypeScript-first schema validation

### Notifications:
- **React Hot Toast** - Beautiful toast notifications

---

## Why Visual Studio for React?

### Pros of Using Visual Studio:
✅ **Single solution** - Backend and frontend in one place
✅ **Unified debugging** - Debug both at the same time
✅ **Project management** - Easier to manage everything together
✅ **IntelliSense** - Full TypeScript support
✅ **Integrated terminal** - Run npm commands directly
✅ **Source control** - Git integration for everything
✅ **Professional workflow** - Similar to enterprise development

### Visual Studio Configuration:
- Visual Studio 2022 has excellent support for React/TypeScript
- Node.js integration works well
- Can run both .NET API and React dev server simultaneously
- Built-in Terminal for npm commands

---

## Project Structure

Your solution structure will be:
```
InventoryManagement/
├── src/
│   ├── Core/
│   │   ├── InventoryManagement.Domain/
│   │   └── InventoryManagement.Application/
│   ├── Infrastructure/
│   │   └── InventoryManagement.Infrastructure/
│   └── Presentation/
│       ├── InventoryManagement.API/              # .NET Web API
│       └── InventoryManagement.Web/              # React Frontend (NEW)
└── tests/
    ├── InventoryManagement.Application.Tests/
    ├── InventoryManagement.Domain.Tests/
    └── InventoryManagement.API.Tests/
```

---

## Technology Stack

### Frontend Technologies:
- **React 18** - UI library
- **TypeScript** - Type safety
- **Vite** - Fast build tool and dev server
- **React Router v6** - Client-side routing
- **TailwindCSS** - Utility-first CSS framework
- **Axios** - HTTP client for API calls
- **React Hook Form** - Form handling and validation
- **React Hot Toast** - Toast notifications
- **Heroicons** - Beautiful icons

### Why These Choices?

**Vite over Create React App**:
- ⚡ Much faster dev server (instant hot reload)
- ⚡ Faster builds
- 🎯 Better TypeScript support out of the box
- 🔧 Modern tooling

**TailwindCSS over Material UI**:
- 🎨 More customizable
- 📦 Smaller bundle size
- 🚀 Faster to prototype
- 💪 Full control over design
- (Material UI adds ~300KB, Tailwind adds ~10KB after purge)

**TypeScript**:
- ✅ Type safety with API responses
- ✅ Better IDE support
- ✅ Catch errors at compile time
- ✅ Self-documenting code

---

## Prerequisites

Before starting, ensure you have:
- ✅ Node.js 18 or later installed ([Download](https://nodejs.org/))
- ✅ Visual Studio 2022 with Node.js development tools
- ✅ Backend API completed and working
- ✅ Git (for version control)

### Verify Node.js Installation:
Open Command Prompt and run:
```bash
node --version
# Should show v18.0.0 or higher

npm --version
# Should show 9.0.0 or higher
```

---

## Task 1: Create React Project with Vite

### Step 1: Open Command Prompt or PowerShell

Navigate to your solution's Presentation folder:
```bash
cd C:\YourPath\InventoryManagement\src\Presentation
```

### Step 2: Create Vite + React + TypeScript Project

Run this command:
```bash
npm create vite@latest InventoryManagement.Web -- --template react-ts
```

**What this does**:
- Creates a new folder `InventoryManagement.Web`
- Sets up React 18 with TypeScript
- Configures Vite for development and production
- Adds basic project structure

### Step 3: Navigate to Project and Install Dependencies

```bash
cd InventoryManagement.Web
npm install
```

### Step 4: Verify Setup

Test that the project works:
```bash
npm run dev
```

You should see:
```
VITE v5.x.x  ready in xxx ms

➜  Local:   http://localhost:5173/
➜  Network: use --host to expose
```

Open browser to `http://localhost:5173/` - you should see the default Vite + React page.

Press `Ctrl+C` to stop the dev server.

---

## Task 2: Install Required Packages

Install all necessary packages for the project:

```bash
# Install TailwindCSS and dependencies
npm install -D tailwindcss@^3.4.0 postcss@^8.4.0 autoprefixer@^10.4.0
npx tailwindcss init -p

# Install routing
npm install react-router-dom

# Install HTTP client
npm install axios

# Install form handling
npm install react-hook-form

# Install toast notifications
npm install react-hot-toast

# Install icons
npm install @heroicons/react

# Install date formatting
npm install date-fns

# Install type definitions
npm install -D @types/node
```

### Verify Installation

Check your `package.json` file - it should include all these packages in `dependencies` and `devDependencies`.

---

## Task 3: Configure TailwindCSS

### Step 1: Update tailwind.config.js

**Location**: `InventoryManagement.Web/tailwind.config.js`

Replace the entire content with:

```javascript
/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          50: '#eff6ff',
          100: '#dbeafe',
          200: '#bfdbfe',
          300: '#93c5fd',
          400: '#60a5fa',
          500: '#3b82f6',
          600: '#2563eb',
          700: '#1d4ed8',
          800: '#1e40af',
          900: '#1e3a8a',
          950: '#172554',
        },
      },
    },
  },
  plugins: [],
}
```

### Step 2: Update CSS File

**Location**: `InventoryManagement.Web/src/index.css`

Replace the entire content with:

```css
@tailwind base;
@tailwind components;
@tailwind utilities;

@layer base {
  body {
    @apply bg-gray-50 text-gray-900;
  }
}

@layer components {
  /* Custom component styles can go here */
}
```

---

## Task 4: Create Project Folder Structure

Create the following folder structure inside `src/`:

```bash
# Run these commands in InventoryManagement.Web folder
mkdir src\components
mkdir src\components\common
mkdir src\components\auth
mkdir src\components\users
mkdir src\pages
mkdir src\services
mkdir src\contexts
mkdir src\types
mkdir src\utils
mkdir src\hooks
```

Your structure should now be:
```
src/
├── components/
│   ├── common/        # Reusable UI components (Button, Input, etc.)
│   ├── auth/          # Authentication components (LoginForm, etc.)
│   └── users/         # User management components
├── pages/             # Page components (LoginPage, UsersPage, etc.)
├── services/          # API service functions (authService, userService)
├── contexts/          # React Context for state management
├── types/             # TypeScript type definitions
├── utils/             # Utility functions (token storage, etc.)
├── hooks/             # Custom React hooks
├── App.tsx
├── main.tsx
└── index.css
```

---

## Task 5: Configure Vite for API Proxy

To avoid CORS issues during development, configure Vite to proxy API requests.

**Location**: `InventoryManagement.Web/vite.config.ts`

**Prompt for Copilot:**

> Update vite.config.ts to add a proxy for API requests following these specifications:
> 
> Import defineConfig from 'vite' and react from '@vitejs/plugin-react'
> 
> Export default configuration with:
> - plugins: [react()]
> - server object with:
>   - port: 5173
>   - proxy object:
>     - '/api' target: 'https://localhost:7xxx' (replace with your API port)
>     - changeOrigin: true
>     - secure: false (for development with self-signed certificates)
> 
> Add comment explaining this proxies /api requests to backend during development

### Find Your API Port:

1. Open `InventoryManagement.API` project in Visual Studio
2. Right-click project → Properties → Debug → Launch Profiles
3. Note the HTTPS port (usually 7xxx)
4. Use this port in the proxy configuration

---

## Task 6: Create TypeScript Type Definitions

### Location: `InventoryManagement.Web/src/types/user.types.ts`

**Prompt for Copilot:**

> Create TypeScript type definitions for User Management following these specifications:
> 
> **File**: user.types.ts
> 
> **Types to create**:
> 
> 1. **UserDto** interface:
>    - id: string
>    - firstName: string
>    - lastName: string
>    - fullName: string
>    - email: string
>    - isActive: boolean
>    - createdAt: string
>    - updatedAt: string
> 
> 2. **AuthenticationResult** interface:
>    - user: UserDto
>    - token: string
> 
> 3. **LoginRequest** interface:
>    - email: string
>    - password: string
> 
> 4. **CreateUserRequest** interface:
>    - firstName: string
>    - lastName: string
>    - email: string
>    - password: string
> 
> 5. **UpdateUserRequest** interface:
>    - userId: string
>    - firstName: string
>    - lastName: string
>    - email: string
> 
> Add JSDoc comments for each interface explaining their purpose

### Location: `InventoryManagement.Web/src/types/api.types.ts`

**Prompt for Copilot:**

> Create TypeScript type definitions for API responses following these specifications:
> 
> **File**: api.types.ts
> 
> **Types to create**:
> 
> 1. **ApiError** interface:
>    - statusCode: number
>    - message: string
>    - errors?: Array<{ property: string; message: string }>
> 
> 2. **ApiResponse<T>** generic type:
>    - data?: T
>    - error?: ApiError
>    - isLoading: boolean
> 
> Add JSDoc comments explaining these are for API error handling

---

## Task 7: Create Environment Configuration

### Location: `InventoryManagement.Web/.env.development`

Create this file and add:

```
VITE_API_BASE_URL=https://localhost:7xxx
```

Replace `7xxx` with your actual API port.

### Location: `InventoryManagement.Web/.env.production`

Create this file and add:

```
VITE_API_BASE_URL=https://your-production-api.com
```

### Note on Environment Variables:

- Vite environment variables must start with `VITE_`
- Access them with `import.meta.env.VITE_API_BASE_URL`
- Never commit `.env.production` with real URLs to Git

---

## Task 8: Update .gitignore

**Location**: `InventoryManagement.Web/.gitignore`

**Prompt for Copilot:**

> Create a comprehensive .gitignore file for React + Vite project with:
> - node_modules
> - dist
> - .env.local, .env.*.local
> - build
> - .DS_Store
> - *.log
> - .vscode (optional)
> - coverage
> 
> Add comments explaining each section

---

## Task 9: Add React Project to Visual Studio Solution

### Option A: Using Visual Studio (Recommended)

1. **Open your solution** in Visual Studio 2022
2. **Right-click** on the `Presentation` folder in Solution Explorer
3. Select **Add** → **Existing Project**
4. Navigate to `InventoryManagement.Web` folder
5. You won't see a .csproj file - instead:
   - Right-click `Presentation` folder
   - Select **Add** → **Existing Web Site**
   - Select the `InventoryManagement.Web` folder
   - Click **Open**

### Option B: Manual Integration (Alternative)

If Visual Studio doesn't recognize it:

1. Right-click `Presentation` folder → **Add** → **New Solution Folder** → Name it "Frontend"
2. Right-click "Frontend" → **Add** → **Existing Item**
3. Navigate to `InventoryManagement.Web`
4. Show all files (*.*)
5. Select `package.json` → **Add as Link**
6. This makes it visible in Solution Explorer

### Configure Solution to Run Both Projects

1. Right-click **Solution** → **Properties**
2. Select **Multiple startup projects**
3. Set `InventoryManagement.API` to **Start**
4. Set `InventoryManagement.Web` to **Start** (if available)
5. Click **OK**

**Note**: Visual Studio may not directly run the React dev server. You'll need to run it manually or create a task.

---

## Task 10: Create NPM Scripts

Update `package.json` scripts for convenience.

**Location**: `InventoryManagement.Web/package.json`

Add/update the scripts section:

```json
"scripts": {
  "dev": "vite",
  "build": "tsc && vite build",
  "lint": "eslint . --ext ts,tsx --report-unused-disable-directives --max-warnings 0",
  "preview": "vite preview",
  "test": "vitest",
  "type-check": "tsc --noEmit"
}
```

---

## Task 11: Clean Up Default Files

Remove or update default Vite files:

### Files to Delete:
- `src/App.css`
- `src/assets/react.svg`
- `public/vite.svg`

### Files to Clear:
- `src/App.tsx` - We'll recreate this in Step 3B
- `src/main.tsx` - We'll update this in Step 3B

Don't delete them yet, just be aware we'll replace them.

---

## Task 12: Verify Setup

### Check 1: Run Development Server

```bash
npm run dev
```

Should start without errors on http://localhost:5173/

### Check 2: Build Project

```bash
npm run build
```

Should complete successfully and create a `dist` folder.

### Check 3: Type Check

```bash
npm run type-check
```

Should pass with no TypeScript errors.

---

## Verification Checklist

Before moving to Step 3B, verify:

- [ ] Node.js 18+ installed
- [ ] React project created in `src/Presentation/InventoryManagement.Web`
- [ ] All packages installed (TailwindCSS, React Router, Axios, etc.)
- [ ] TailwindCSS configured
- [ ] Folder structure created (components, pages, services, etc.)
- [ ] Vite proxy configured with correct API port
- [ ] TypeScript type definitions created
- [ ] Environment variables configured
- [ ] .gitignore updated
- [ ] Project added to Visual Studio solution (or accessible)
- [ ] `npm run dev` works without errors
- [ ] `npm run build` completes successfully
- [ ] API backend is running and accessible

---

## Running Both Projects Together

### Method 1: Two Terminal Windows

**Terminal 1 - API**:
```bash
cd src/Presentation/InventoryManagement.API
dotnet run
```

**Terminal 2 - React**:
```bash
cd src/Presentation/InventoryManagement.Web
npm run dev
```

### Method 2: Visual Studio + Command Prompt

1. Run API from Visual Studio (F5)
2. Open Command Prompt, navigate to Web folder
3. Run `npm run dev`

### Method 3: VS Code Tasks (Alternative)

If you prefer VS Code for React development:
1. Open `InventoryManagement.Web` in VS Code
2. Copilot can help create `.vscode/tasks.json` to run both
3. Use integrated terminal to run API separately

---

## Project Structure Overview

Your complete structure now:

```
InventoryManagement/
├── src/
│   ├── Core/
│   │   ├── InventoryManagement.Domain/
│   │   └── InventoryManagement.Application/
│   ├── Infrastructure/
│   │   └── InventoryManagement.Infrastructure/
│   └── Presentation/
│       ├── InventoryManagement.API/
│       │   ├── Controllers/
│       │   ├── Program.cs
│       │   └── appsettings.json
│       └── InventoryManagement.Web/           # ✅ NEW
│           ├── public/
│           ├── src/
│           │   ├── components/
│           │   ├── pages/
│           │   ├── services/
│           │   ├── contexts/
│           │   ├── types/
│           │   ├── utils/
│           │   ├── hooks/
│           │   ├── App.tsx
│           │   ├── main.tsx
│           │   └── index.css
│           ├── package.json
│           ├── tsconfig.json
│           ├── vite.config.ts
│           ├── tailwind.config.js
│           └── .env.development
└── tests/
```

---

## What You've Accomplished

✅ **React project created** with Vite + TypeScript  
✅ **TailwindCSS configured** for styling  
✅ **Project structure** organized and ready  
✅ **API proxy** configured for development  
✅ **Type definitions** created for type safety  
✅ **Development environment** fully set up  
✅ **Integrated with Visual Studio** solution  

---

## Common Issues and Solutions

**Issue**: "npm: command not found"
- Solution: Install Node.js from nodejs.org, restart terminal

**Issue**: "Port 5173 already in use"
- Solution: Change port in vite.config.ts server.port

**Issue**: "Cannot find module '@types/react'"
- Solution: Run `npm install` again

**Issue**: "TailwindCSS not working"
- Solution: Verify tailwind.config.js content paths, restart dev server

**Issue**: "API CORS errors"
- Solution: Verify Vite proxy configuration matches API port, check API CORS policy

**Issue**: "Visual Studio won't start React project"
- Solution: Run React manually with `npm run dev`, focus VS on API project

---

## Next Steps

You're now ready for **Step 3B: Authentication UI**!

In Step 3B, you'll create:
1. ✅ Login page with form validation
2. ✅ JWT token storage and management
3. ✅ Authentication context for state management
4. ✅ Protected routes (require login)
5. ✅ API service with Axios interceptors
6. ✅ Error handling and toast notifications
7. ✅ Automatic token refresh logic

Then in Step 3C, you'll build the full User Management UI!

---

## Tips for Working with React + Copilot

1. **Component prompts**: "Create a Button component with TailwindCSS that accepts..."
2. **Type safety**: Always define TypeScript types before components
3. **Ask for explanations**: "Explain this useEffect hook"
4. **Styling help**: "Make this form look better with Tailwind"
5. **Error handling**: "Add try-catch to this API call"

---

## Resources

- **Vite**: https://vitejs.dev/
- **React**: https://react.dev/
- **TailwindCSS**: https://tailwindcss.com/
- **React Router**: https://reactrouter.com/
- **Axios**: https://axios-http.com/

---

## Summary

You've set up a modern React frontend project with:
- ⚡ Vite for fast development
- 🎨 TailwindCSS for beautiful styling
- 🔒 TypeScript for type safety
- 🛣️ React Router for navigation
- 📡 Axios for API calls
- 🏗️ Clean folder structure
- ✅ Integrated with your Visual Studio solution

Your frontend is ready to connect to your backend API!
