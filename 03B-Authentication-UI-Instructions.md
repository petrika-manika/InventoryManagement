# Step 3B: Authentication UI - Login and JWT Management
## Instructions for GitHub Copilot

---

## Overview
This guide provides instructions for building the authentication system for your React frontend. You'll create a login page, JWT token management, authentication context, and protected routes.

**Important**: This file contains instructions for GitHub Copilot to generate React components and authentication logic.

---

## What We're Building

### Authentication Features:
- 🔐 **Login Page** - Beautiful, responsive login form
- 🎯 **Form Validation** - Client-side validation with helpful error messages
- 🔑 **JWT Token Management** - Secure token storage and retrieval
- 🛡️ **Protected Routes** - Automatic redirect if not authenticated
- 🔄 **Authentication Context** - Global auth state management
- 📡 **API Service** - Axios instance with interceptors
- 🎨 **Loading States** - User-friendly loading indicators
- 🔔 **Toast Notifications** - Success/error messages
- 🚪 **Logout** - Clear session and redirect

---

## Architecture Overview

```
Authentication Flow:
1. User enters credentials → LoginPage
2. Submit → authService.login() → API call
3. Success → Store JWT in localStorage → Update AuthContext
4. Navigate → Protected routes now accessible
5. API calls → Axios adds JWT header automatically
6. Logout → Clear token → Redirect to login
```

---

## Task 1: Create Utility Functions

### Task 1.1: Token Storage Utilities

**Location**: `src/utils/tokenStorage.ts`

**Prompt for Copilot:**

> Create token storage utility functions following these specifications:
> 
> **Purpose**: Manage JWT token in localStorage
> 
> **Constants**:
> - TOKEN_KEY = 'auth_token'
> 
> **Functions to export**:
> 
> 1. **getToken()** → string | null:
>    - Returns token from localStorage.getItem(TOKEN_KEY)
> 
> 2. **setToken(token: string)** → void:
>    - Saves token to localStorage.setItem(TOKEN_KEY, token)
> 
> 3. **removeToken()** → void:
>    - Removes token with localStorage.removeItem(TOKEN_KEY)
> 
> 4. **isTokenValid()** → boolean:
>    - Gets token
>    - If no token, return false
>    - Decode JWT payload (split by '.', base64 decode middle part)
>    - Check if exp (expiration) is greater than current timestamp
>    - Return true if valid, false if expired
>    - Include try-catch for decode errors
> 
> Add JSDoc comments explaining each function
> Add comment about using jwt-decode library as alternative

### Task 1.2: API Configuration

**Location**: `src/utils/apiConfig.ts`

**Prompt for Copilot:**

> Create API configuration utility following these specifications:
> 
> **Purpose**: Centralized API configuration
> 
> **Export constants**:
> - API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'https://localhost:7xxx'
> - API_ENDPOINTS object with:
>   - AUTH object:
>     - LOGIN: '/api/auth/login'
>   - USERS object:
>     - BASE: '/api/users'
>     - GET_ALL: '/api/users'
>     - GET_BY_ID: (id: string) => `/api/users/${id}`
>     - GET_CURRENT: '/api/users/me'
>     - CREATE: '/api/users'
>     - UPDATE: (id: string) => `/api/users/${id}`
>     - ACTIVATE: (id: string) => `/api/users/${id}/activate`
>     - DEACTIVATE: (id: string) => `/api/users/${id}/deactivate`
> 
> Add JSDoc comments explaining endpoint structure

---

## Task 2: Create Axios Service

**Location**: `src/services/apiClient.ts`

**Prompt for Copilot:**

> Create Axios client with interceptors following these specifications:
> 
> **Imports**: axios, getToken and removeToken from tokenStorage, API_BASE_URL from apiConfig
> 
> **Create Axios Instance**:
> - baseURL: API_BASE_URL
> - headers: { 'Content-Type': 'application/json' }
> - timeout: 10000 (10 seconds)
> 
> **Request Interceptor**:
> - Get token using getToken()
> - If token exists, add to headers: Authorization: `Bearer ${token}`
> - Return config
> - On error, reject with error
> 
> **Response Interceptor**:
> - On success response (2xx), return response
> - On error response:
>   - If status 401 (Unauthorized):
>     - Remove token using removeToken()
>     - Redirect to /login (window.location.href = '/login')
>   - Extract error message from response.data.message or use generic message
>   - Create formatted error object with: statusCode, message, errors
>   - Reject with formatted error
> 
> Export the apiClient instance as default
> 
> Add JSDoc comments explaining interceptor logic

---

## Task 3: Create Authentication Service

**Location**: `src/services/authService.ts`

**Prompt for Copilot:**

> Create authentication service following these specifications:
> 
> **Imports**: 
> - apiClient from './apiClient'
> - API_ENDPOINTS from '../utils/apiConfig'
> - Types: LoginRequest, AuthenticationResult from '../types/user.types'
> - setToken, removeToken from '../utils/tokenStorage'
> 
> **authService object with methods**:
> 
> 1. **login(credentials: LoginRequest)** → Promise<AuthenticationResult>:
>    - Make POST request to API_ENDPOINTS.AUTH.LOGIN with credentials
>    - On success:
>      - Extract response.data
>      - Call setToken(data.token)
>      - Return data (AuthenticationResult)
>    - Let errors propagate (caught by interceptor)
> 
> 2. **logout()** → void:
>    - Call removeToken()
>    - Redirect to /login: window.location.href = '/login'
> 
> 3. **getCurrentUser()** → Promise<UserDto>:
>    - Make GET request to API_ENDPOINTS.USERS.GET_CURRENT
>    - Return response.data
> 
> Export authService as default
> Add JSDoc comments for each method

---

## Task 4: Create Authentication Context

**Location**: `src/contexts/AuthContext.tsx`

**Prompt for Copilot:**

> Create React Context for authentication state following these specifications:
> 
> **Imports**: 
> - React (createContext, useState, useEffect, useContext, ReactNode)
> - UserDto from '../types/user.types'
> - authService from '../services/authService'
> - getToken, isTokenValid from '../utils/tokenStorage'
> 
> **AuthContextType interface**:
> - user: UserDto | null
> - isAuthenticated: boolean
> - isLoading: boolean
> - login: (email: string, password: string) => Promise<void>
> - logout: () => void
> 
> **Create AuthContext**:
> - Use createContext<AuthContextType | undefined>(undefined)
> 
> **AuthProvider component**:
> - Props: { children: ReactNode }
> - State:
>   - user: UserDto | null (useState)
>   - isLoading: boolean (useState, initial true)
> - Computed:
>   - isAuthenticated = user !== null
> 
> **useEffect on mount**:
> - Check if token exists and is valid using getToken() and isTokenValid()
> - If valid:
>   - Try to fetch current user with authService.getCurrentUser()
>   - On success: setUser(userData)
>   - On error: authService.logout() (invalid token)
> - Set isLoading(false) in finally block
> 
> **login function**:
> - Parameters: email, password
> - Call authService.login({ email, password })
> - On success: setUser(result.user)
> - Let errors bubble up (caller handles)
> 
> **logout function**:
> - Set user to null
> - Call authService.logout()
> 
> **Context value**:
> - { user, isAuthenticated, isLoading, login, logout }
> 
> **Return**:
> - <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
> 
> **Export custom hook - useAuth**:
> - Uses useContext(AuthContext)
> - Throws error if used outside AuthProvider
> - Returns context value
> 
> Export AuthProvider and useAuth
> Add JSDoc comments explaining context purpose and usage

---

## Task 5: Create Common UI Components

### Task 5.1: Button Component

**Location**: `src/components/common/Button.tsx`

**Prompt for Copilot:**

> Create reusable Button component with TailwindCSS following these specifications:
> 
> **Imports**: React, ReactNode
> 
> **Props interface (ButtonProps)**:
> - children: ReactNode
> - type?: 'button' | 'submit' | 'reset' (default 'button')
> - variant?: 'primary' | 'secondary' | 'danger' (default 'primary')
> - size?: 'sm' | 'md' | 'lg' (default 'md')
> - fullWidth?: boolean (default false)
> - disabled?: boolean (default false)
> - isLoading?: boolean (default false)
> - onClick?: () => void
> - className?: string (for additional custom classes)
> 
> **Styling logic**:
> - Base classes: 'font-semibold rounded-lg transition-colors duration-200 focus:outline-none focus:ring-2 focus:ring-offset-2'
> - Variant classes:
>   - primary: 'bg-primary-600 hover:bg-primary-700 text-white focus:ring-primary-500'
>   - secondary: 'bg-gray-200 hover:bg-gray-300 text-gray-900 focus:ring-gray-500'
>   - danger: 'bg-red-600 hover:bg-red-700 text-white focus:ring-red-500'
> - Size classes:
>   - sm: 'px-3 py-1.5 text-sm'
>   - md: 'px-4 py-2 text-base'
>   - lg: 'px-6 py-3 text-lg'
> - Disabled: 'opacity-50 cursor-not-allowed'
> - Full width: 'w-full'
> 
> **Loading state**:
> - When isLoading true, show spinner icon and "Loading..." text
> - Disable button when isLoading or disabled
> 
> **Return**:
> - Button element with computed classes, disabled state, type, onClick
> - Show spinner if loading, otherwise show children
> 
> Export Button as default
> Add JSDoc comments explaining props

### Task 5.2: Input Component

**Location**: `src/components/common/Input.tsx`

**Prompt for Copilot:**

> Create reusable Input component with TailwindCSS following these specifications:
> 
> **Imports**: React, forwardRef
> 
> **Props interface (InputProps)** extends React.InputHTMLAttributes<HTMLInputElement>:
> - label?: string
> - error?: string
> - helperText?: string
> 
> **Component**:
> - Use forwardRef for ref forwarding (needed for react-hook-form)
> - Accept all standard input props via spread
> 
> **Styling**:
> - Container: div with mb-4
> - Label: if provided, show with 'block text-sm font-medium text-gray-700 mb-1'
> - Input base: 'w-full px-3 py-2 border rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-offset-0 transition-colors'
> - Input normal: 'border-gray-300 focus:border-primary-500 focus:ring-primary-500'
> - Input error: 'border-red-300 focus:border-red-500 focus:ring-red-500'
> - Error message: if provided, show with 'mt-1 text-sm text-red-600'
> - Helper text: if provided and no error, show with 'mt-1 text-sm text-gray-500'
> 
> **Return**:
> - Wrapper div
> - Label if provided
> - Input with ref, computed classes, spread props
> - Error message if error
> - Helper text if helperText and no error
> 
> Export Input as default with forwardRef
> Add JSDoc comments

### Task 5.3: Loading Spinner

**Location**: `src/components/common/LoadingSpinner.tsx`

**Prompt for Copilot:**

> Create LoadingSpinner component following these specifications:
> 
> **Props**:
> - size?: 'sm' | 'md' | 'lg' (default 'md')
> - className?: string
> 
> **Sizing**:
> - sm: 'h-4 w-4'
> - md: 'h-8 w-8'
> - lg: 'h-12 w-12'
> 
> **SVG spinner**:
> - Animated spinning circle
> - Use TailwindCSS animate-spin
> - Classes: 'animate-spin text-primary-600'
> - SVG viewBox: "0 0 24 24"
> - Circle with stroke dasharray for spinner effect
> 
> Or use a simple div-based spinner with border animation
> 
> Export LoadingSpinner as default
> Add JSDoc comment

---

## Task 6: Create Login Page

**Location**: `src/pages/LoginPage.tsx`

**Prompt for Copilot:**

> Create LoginPage component with form validation following these specifications:
> 
> **Imports**:
> - React (useState)
> - useNavigate from 'react-router-dom'
> - useAuth from '../contexts/AuthContext'
> - toast from 'react-hot-toast'
> - Button from '../components/common/Button'
> - Input from '../components/common/Input'
> 
> **Component state**:
> - email: string (useState)
> - password: string (useState)
> - isLoading: boolean (useState, initial false)
> - errors: { email?: string; password?: string } (useState)
> 
> **Hooks**:
> - const { login } = useAuth()
> - const navigate = useNavigate()
> 
> **Validation function - validateForm()**:
> - Returns boolean
> - Create errors object
> - Email validation:
>   - Check if empty: "Email is required"
>   - Check format with regex: "Invalid email format"
> - Password validation:
>   - Check if empty: "Password is required"
>   - Check length >= 6: "Password must be at least 6 characters"
> - setErrors(errors)
> - Return true if no errors, false otherwise
> 
> **Submit handler - handleSubmit(e: FormEvent)**:
> - e.preventDefault()
> - Clear previous errors: setErrors({})
> - If !validateForm(), return
> - setIsLoading(true)
> - Try:
>   - await login(email, password)
>   - toast.success('Login successful!')
>   - navigate('/dashboard') or navigate('/users')
> - Catch error:
>   - toast.error(error.message || 'Login failed')
> - Finally:
>   - setIsLoading(false)
> 
> **JSX Structure**:
> - Full-screen centered container with gradient background
> - White card with shadow and rounded corners
> - Logo or app title at top
> - Heading: "Sign in to your account"
> - Form with onSubmit={handleSubmit}:
>   - Input for email:
>     - label="Email address"
>     - type="email"
>     - value={email}
>     - onChange={e => setEmail(e.target.value)}
>     - error={errors.email}
>     - placeholder="you@example.com"
>     - autoComplete="email"
>   - Input for password:
>     - label="Password"
>     - type="password"
>     - value={password}
>     - onChange={e => setPassword(e.target.value)}
>     - error={errors.password}
>     - placeholder="••••••••"
>     - autoComplete="current-password"
>   - Submit Button:
>     - type="submit"
>     - fullWidth
>     - isLoading={isLoading}
>     - children="Sign in"
> - Optional: "Don't have an account?" link (if you add registration later)
> 
> **Styling**:
> - Use TailwindCSS utilities
> - Modern, clean design
> - Responsive (mobile-friendly)
> - Add subtle hover effects
> - Use primary color scheme
> 
> Export LoginPage as default
> Add JSDoc comment

---

## Task 7: Create Protected Route Component

**Location**: `src/components/auth/ProtectedRoute.tsx`

**Prompt for Copilot:**

> Create ProtectedRoute component following these specifications:
> 
> **Imports**:
> - React (ReactNode)
> - Navigate from 'react-router-dom'
> - useAuth from '../../contexts/AuthContext'
> - LoadingSpinner from '../common/LoadingSpinner'
> 
> **Props**:
> - children: ReactNode
> 
> **Component logic**:
> - const { isAuthenticated, isLoading } = useAuth()
> 
> **Return logic**:
> - If isLoading: return full-screen centered LoadingSpinner
> - If !isAuthenticated: return <Navigate to="/login" replace />
> - Otherwise: return {children}
> 
> **Full-screen loading**:
> - Container: 'flex items-center justify-center min-h-screen'
> - Center LoadingSpinner with size="lg"
> 
> Export ProtectedRoute as default
> Add JSDoc comment explaining this protects routes requiring authentication

---

## Task 8: Update App.tsx with Routing

**Location**: `src/App.tsx`

**Prompt for Copilot:**

> Update App.tsx to set up routing following these specifications:
> 
> **Imports**:
> - React
> - BrowserRouter, Routes, Route, Navigate from 'react-router-dom'
> - Toaster from 'react-hot-toast'
> - AuthProvider from './contexts/AuthContext'
> - ProtectedRoute from './components/auth/ProtectedRoute'
> - LoginPage from './pages/LoginPage'
> 
> **Structure**:
> - Wrap entire app with BrowserRouter
> - Inside BrowserRouter, wrap with AuthProvider
> - Inside AuthProvider:
>   - Add <Toaster position="top-right" /> for notifications
>   - Routes container:
>     - Route path="/login" element={<LoginPage />}
>     - Route path="/" element={<Navigate to="/users" replace />} (temporary, redirects to users)
>     - Protected routes (wrap with ProtectedRoute):
>       - Route path="/users" element={<div>Users Page (Coming in Step 3C)</div>}
>       - Route path="/dashboard" element={<div>Dashboard (Coming later)</div>}
>     - Route path="*" element={<Navigate to="/login" replace />} (404 redirect)
> 
> **Toaster configuration**:
> - position="top-right"
> - toastOptions with success and error styles (green and red)
> 
> Export App as default
> Add comments explaining route structure

---

## Task 9: Update main.tsx

**Location**: `src/main.tsx`

**Prompt for Copilot:**

> Update main.tsx following these specifications:
> 
> **Imports**:
> - React
> - ReactDOM from 'react-dom/client'
> - App from './App'
> - './index.css'
> 
> **Code**:
> - ReactDOM.createRoot(document.getElementById('root')!)
> - Render:
>   - <React.StrictMode>
>     - <App />
>   - </React.StrictMode>
> 
> Keep it simple and clean

---

## Task 10: Testing the Authentication

### Manual Testing Steps:

**Test 1: Login Flow**
1. Start both API and React:
   - API: Run from Visual Studio (F5)
   - React: `npm run dev` in InventoryManagement.Web folder
2. Navigate to http://localhost:5173
3. Should redirect to /login automatically (not authenticated)
4. Try logging in with admin credentials:
   - Email: admin@inventoryapp.com
   - Password: Admin@123
5. Should show success toast
6. Should redirect to /users page
7. Should see "Users Page" placeholder

**Test 2: Protected Routes**
1. While logged in, manually navigate to http://localhost:5173/dashboard
2. Should see "Dashboard" placeholder (not redirect to login)
3. Open browser DevTools → Application → Local Storage
4. Should see `auth_token` with JWT value

**Test 3: Logout**
1. While logged in, open browser console
2. Type: `localStorage.removeItem('auth_token')`
3. Refresh page
4. Should redirect to login

**Test 4: Invalid Credentials**
1. Try logging in with wrong password
2. Should show error toast: "Invalid email or password"
3. Should stay on login page

**Test 5: Form Validation**
1. Try submitting empty form
2. Should show validation errors
3. Try invalid email format
4. Should show "Invalid email format"
5. Try password less than 6 characters
6. Should show password length error

**Test 6: Token Expiration**
1. Login successfully
2. Wait 24 hours (or modify token expiration in backend for testing)
3. Try navigating to protected route
4. Should redirect to login (token expired)

**Test 7: API Call with Token**
1. Login successfully
2. Open browser DevTools → Network tab
3. Navigate to /users page (will make API call in Step 3C)
4. Check request headers
5. Should see: `Authorization: Bearer <token>`

---

## Verification Checklist

Before moving to Step 3C, verify:

- [ ] Token storage utilities created (get, set, remove, validate)
- [ ] API configuration created with all endpoints
- [ ] Axios client created with interceptors
- [ ] Authentication service created (login, logout, getCurrentUser)
- [ ] Authentication context created (AuthProvider, useAuth)
- [ ] Common components created (Button, Input, LoadingSpinner)
- [ ] Login page created with validation
- [ ] Protected route component created
- [ ] App.tsx updated with routes
- [ ] main.tsx updated
- [ ] Can login with valid credentials
- [ ] Invalid credentials show error
- [ ] Form validation works
- [ ] Token stored in localStorage after login
- [ ] Protected routes redirect to login when not authenticated
- [ ] Protected routes accessible when authenticated
- [ ] Logout clears token and redirects
- [ ] Toast notifications work
- [ ] No console errors

---

## What You've Accomplished

✅ **Complete authentication system**  
✅ **JWT token management** - Secure storage and validation  
✅ **Login page** - Beautiful, validated form  
✅ **Protected routes** - Automatic authentication checks  
✅ **Global auth state** - Context API for user info  
✅ **API integration** - Axios with interceptors  
✅ **Error handling** - User-friendly messages  
✅ **Loading states** - Professional UX  

---

## Authentication Flow Summary

```
1. User opens app
   ↓
2. App checks for token in localStorage
   ↓
3a. No token → Redirect to /login
3b. Valid token → Fetch user data → Stay on current route
   ↓
4. User logs in
   ↓
5. API returns JWT token
   ↓
6. Store token in localStorage
   ↓
7. Update AuthContext with user data
   ↓
8. Navigate to protected route
   ↓
9. All API calls automatically include token
   ↓
10. User logs out → Clear token → Redirect to login
```

---

## Security Considerations

✅ **Token in localStorage**: Acceptable for this application  
⚠️ **Production**: Consider httpOnly cookies for extra security  
✅ **Token expiration**: Backend handles (24 hours)  
✅ **Automatic logout**: On 401 responses  
✅ **HTTPS only**: In production  
⚠️ **XSS protection**: Sanitize user inputs (React does this by default)  
⚠️ **CSRF protection**: Not needed with JWT (stateless)  

---

## Common Issues and Solutions

**Issue**: "CORS error on login"
- Solution: Check API CORS policy includes React origin, verify Vite proxy

**Issue**: "Token not being sent with requests"
- Solution: Check Axios interceptor, verify token in localStorage

**Issue**: "Infinite redirect loop"
- Solution: Check ProtectedRoute logic, verify AuthContext isLoading

**Issue**: "User data not loading after refresh"
- Solution: Check useEffect in AuthContext, verify token validity check

**Issue**: "Toast notifications not showing"
- Solution: Verify Toaster component in App.tsx, check toast import

**Issue**: "Form validation not working"
- Solution: Check validateForm function, verify errors state

---

## Next Steps

You're ready for **Step 3C: User Management UI**!

In Step 3C, you'll create:
1. ✅ Users list page with table
2. ✅ Create user modal/form
3. ✅ Edit user modal/form
4. ✅ Activate/Deactivate user actions
5. ✅ Delete confirmation modal
6. ✅ User details view
7. ✅ Search and filter
8. ✅ Pagination
9. ✅ User service for API calls
10. ✅ Complete CRUD operations

---

## Tips for Step 3C

1. **Reuse components**: Button, Input, LoadingSpinner already created
2. **Follow same patterns**: State management, error handling, loading states
3. **Use Copilot**: For table components, modals, forms
4. **Test incrementally**: Get list working, then create, then edit, etc.
5. **TailwindCSS**: Use utility classes for consistent styling

---

## Summary

Your authentication system is complete and production-ready:
- 🔐 Secure JWT authentication
- 🎨 Beautiful, responsive login page
- 🛡️ Protected routes
- 🔄 Global state management
- 📡 Automatic token handling
- 🔔 User-friendly notifications
- ✨ Professional UX with loading states

You're ready to build the full User Management UI! 🚀
