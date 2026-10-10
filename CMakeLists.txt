cmake_minimum_required(VERSION 3.10)
project(LearnOpenGL C CXX)

# Set C++ standard
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

cmake_minimum_required(VERSION 3.10)
project(LearnOpenGL LANGUAGES C CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

include(FetchContent)

FetchContent_Declare(
    glm
    URL https://github.com/g-truc/glm/archive/refs/tags/1.0.1.tar.gz
)
FetchContent_MakeAvailable(glm)

find_package(OpenGL REQUIRED)

add_executable(LearnOpenGL
    src/main.cpp
    src/glad.c
)

target_include_directories(LearnOpenGL PRIVATE
    ${CMAKE_SOURCE_DIR}/include
    ${CMAKE_SOURCE_DIR}/src
    ${glm_SOURCE_DIR}
)

target_link_libraries(LearnOpenGL PRIVATE
    ${CMAKE_SOURCE_DIR}/lib/libglfw3dll.a
    OpenGL::GL
)

add_custom_command(TARGET LearnOpenGL POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy_if_different
    "${CMAKE_SOURCE_DIR}/glfw3.dll"
    $<TARGET_FILE_DIR:LearnOpenGL>
    COMMAND ${CMAKE_COMMAND} -E copy_if_different
    "${CMAKE_SOURCE_DIR}/src/shader.vs"
    $<TARGET_FILE_DIR:LearnOpenGL>
    COMMAND ${CMAKE_COMMAND} -E copy_if_different
    "${CMAKE_SOURCE_DIR}/src/shader.fs"
    $<TARGET_FILE_DIR:LearnOpenGL>
)