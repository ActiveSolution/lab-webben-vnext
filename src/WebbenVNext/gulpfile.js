var gulp = require('gulp');
var ts = require('gulp-typescript');

gulp.task('default', function () {
    return gulp.src('src/**/*.ts')
        .pipe(ts({
            out: 'app.js'
        }))
        .pipe(gulp.dest('wwwroot/dist'));
});

gulp.task('watch', function () {
    gulp.watch('src/**/*.ts', ['default']);
});