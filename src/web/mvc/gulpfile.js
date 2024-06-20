const { watch } = require('gulp');
var gulp = require('gulp'),
    sass = require('gulp-sass')(require('sass'))
cssmin = require("gulp-cssmin")
rename = require("gulp-rename");

gulp.task('watch', function (done) {
    // All events will be watched
    watch('assets/scss/inc/*.scss', { events: 'all' }, function (cb) {
        gulp.src('assets/scss/style.scss')
            .pipe(sass().on('error', sass.logError))
            .pipe(cssmin())
            .pipe(rename({
                suffix: ".min"
            }))
            .pipe(gulp.dest('wwwroot/assets/css'));
        cb();
    });
});

gulp.task('min', function (done) {
    gulp.src('assets/scss/style.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(cssmin())
        .pipe(rename({
            suffix: ".min"
        }))
        .pipe(gulp.dest('wwwroot/assets/css'));
    done();
});

