document.getElementById('angle').addEventListener('input', validateInputs);
document.getElementById('velocity').addEventListener('input', validateInputs);
document.getElementById('launch').addEventListener('click', launchProjectile);

let score = 0;
let level = 1;
const gameAreaWidth = 600;
const gameAreaHeight = 300;
const gravity = 9.8;
let targets = [];
let timeLeft = 60; // Initial timer value for Level 1
let timerInterval;

// Initialize the game
function initGame() {
    displayBall(); // Display the ball first
    createTargets();
    updateScore();
    updateLevel();
    startTimer(); // Start the timer
}

// Display the ball at its initial position
function displayBall() {
    const projectile = document.getElementById('projectile');
    projectile.style.display = 'block';
    projectile.style.left = '50px';
    projectile.style.bottom = '0';

    // Enable inputs after the ball is displayed
    setTimeout(() => {
        document.getElementById('angle').disabled = false;
        document.getElementById('velocity').disabled = false;
    }, 500); // Small delay for better UX
}

// Create multiple targets at random positions
function createTargets() {
    const gameArea = document.querySelector('.game-area');
    targets = [];
    for (let i = 0; i < level + 1; i++) {
        const target = document.createElement('div');
        target.className = 'target';
        const targetPosition = Math.random() * (gameAreaWidth - 100) + 50; // Random position
        target.style.left = `${targetPosition}px`;
        targets.push({ element: target, position: targetPosition, hit: false });
        gameArea.appendChild(target);
    }
}

// Validate inputs and enable/disable the launch button
function validateInputs() {
    const angle = document.getElementById('angle').value;
    const velocity = document.getElementById('velocity').value;
    const launchButton = document.getElementById('launch');

    if (angle && velocity) {
        launchButton.disabled = false;
    } else {
        launchButton.disabled = true;
    }
}

// Launch the projectile
function launchProjectile() {
    const angle = parseFloat(document.getElementById('angle').value);
    const velocity = parseFloat(document.getElementById('velocity').value);
    const angleRad = (angle * Math.PI) / 180;

    // Calculate range with wind resistance (simplified)
    const windResistance = level * 0.1; // Wind resistance increases with level
    const range = (Math.pow(velocity, 2) * Math.sin(2 * angleRad)) / (gravity + windResistance);

    // Animate the projectile
    const projectile = document.getElementById('projectile');
    projectile.style.display = 'block';
    projectile.style.left = '50px';
    projectile.style.bottom = '0';

    const animationDuration = 2; // seconds
    const startTime = Date.now();

    function animate() {
        const currentTime = Date.now();
        const elapsedTime = (currentTime - startTime) / 1000;

        if (elapsedTime < animationDuration) {
            const progress = elapsedTime / animationDuration;
            const x = 50 + (range * (gameAreaWidth / 100)) * progress;
            const y = gameAreaHeight * (1 - progress);

            projectile.style.left = `${x}px`;
            projectile.style.bottom = `${y}px`;

            requestAnimationFrame(animate);
        } else {
            projectile.style.display = 'none';
            checkHit(range);
        }
    }

    animate();
}

// Check if the projectile hits any target
function checkHit(range) {
    const resultElement = document.getElementById('result');
    let hitAnyTarget = false;

    targets.forEach((target) => {
        if (!target.hit && Math.abs(target.position - (50 + (range * (gameAreaWidth / 100)))) < 20) {
            target.hit = true;
            target.element.style.backgroundColor = 'orange'; // Mark hit target
            score += 10;
            hitAnyTarget = true;
        }
    });

    if (hitAnyTarget) {
        resultElement.textContent = 'Hit!';
        updateScore();
        if (targets.every((target) => target.hit)) {
            levelUp();
        }
    } else {
        resultElement.textContent = 'Miss!';
    }
}

// Update the score
function updateScore() {
    document.getElementById('score').textContent = `Score: ${score}`;
}

// Level up
function levelUp() {
    level++;
    timeLeft += 60; // Add 60 seconds to the timer
    updateLevel();
    document.getElementById('result').textContent = 'Level Up!';
    resetTimer(); // Reset the timer with the new time limit
    setTimeout(() => {
        clearTargets();
        createTargets(); // Create new targets at random positions
    }, 1000);
}

// Clear existing targets
function clearTargets() {
    targets.forEach((target) => target.element.remove());
}

// Update the level display
function updateLevel() {
    document.getElementById('level').textContent = `Level: ${level}`;
}

// Start the timer
function startTimer() {
    timerInterval = setInterval(() => {
        timeLeft--;
        document.getElementById('timer').textContent = `Time: ${timeLeft}`;

        if (timeLeft <= 0) {
            clearInterval(timerInterval);
            gameOver();
        }
    }, 1000); // Update every second
}

// Reset the timer
function resetTimer() {
    clearInterval(timerInterval);
    document.getElementById('timer').textContent = `Time: ${timeLeft}`;
    startTimer(); // Restart the timer
}

// Game over logic
function gameOver() {
    document.getElementById('result').textContent = 'Game Over!';
    setTimeout(() => {
        resetGame(); // Reset the game after a delay
    }, 2000);
}

// Reset the game
function resetGame() {
    score = 0;
    level = 1;
    timeLeft = 60; // Reset to initial timer value
    clearTargets();
    initGame(); // Reinitialize the game
}

// Initialize the game when the page loads
initGame();