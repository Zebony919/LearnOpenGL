cmake_minimum_required(VERSION 3.10)
project(LearnOpenGL C CXX)

# Set C++ standard
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Include directories
include_directories(include)

# Find OpenGL library (system dependency)
find_package(OpenGL REQUIRED)

# Add executable with source files
add_executable(LearnOpenGL
    src/main.cpp
    src/glad.c
)

# Link GLFW library (using lib/libglfw3dll.a) and OpenGL
target_link_libraries(LearnOpenGL
    ${CMAKE_SOURCE_DIR}/lib/libglfw3dll.a
    OpenGL::GL
)

# Post-build step: Copy glfw3.dll to output directory so the executable can run
add_custom_command(TARGET LearnOpenGL POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy_if_different
    "${CMAKE_SOURCE_DIR}/glfw3.dll"
    $<TARGET_FILE_DIR:LearnOpenGL>
)