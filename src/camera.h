#ifndef CAMERA_H
#define CAMERA_H

#include <glad/glad.h>
#include <glm/glm.hpp>
#include <glm/gtc/type_ptr.hpp>

class Camera {
    private:
        glm::vec3 pos;
        glm::vec3 front;
        glm::vec3 up;
        float fov;

    public:
        Camera(glm::vec3 pos, glm::vec3 front, glm::vec3 up, float fov) :
                pos(pos), front(front), up(up), fov(fov) {}

        void setPos(glm::vec3 newPos) { pos = newPos; }
        glm::vec3 getPos() { return pos; }

        void setFront(glm::vec3 newFront) { front = newFront; }
        glm::vec3 getFront() { return front; }

        void setUp(glm::vec3 newUp) { up = newUp; }
        glm::vec3 getUp() { return up; }

        void setFov(float newFov) { fov = newFov; }
        float getFov() { return fov; }
};

#endif