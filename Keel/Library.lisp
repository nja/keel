(defun null (x) (eq x nil))

(defun not (x)
  (if (eq x nil)
    t
    nil))

(defun length (lst)
  (if (null lst)
      0
      (+ 1 (length (cdr lst)))))

(defun list x x)

(defun list* args
  (cond ((null (cdr args))
         (car args))
        ((null (cddr args))
         (cons (car args)
               (cadr args)))
        (t (cons (car args)
                 (apply list*
                        (cadr args)
                        (cddr args))))))

(defun caar (x) (car (car x)))
(defun cadr (x) (car (cdr x)))
(defun cdar (x) (cdr (car x)))
(defun cddr (x) (cdr (cdr x)))
(defun caaar (x) (car (car (car x))))
(defun cdaar (x) (cdr (car (car x))))

(defmacro cond args
  (when (car args)
    (list 'if (caar args)
          (cons 'progn (cdar args))
          (cons 'cond (cdr args)))))

(defun reverse (lst)
  (defun recur (acc lst)
    (if (null lst)
        acc
        (recur (cons (car lst) acc) (cdr lst))))
  (recur nil lst))

(defun some (test list)
  (if (null list)
      nil
      (if (test (car list))
          t
          (some test (cdr list)))))

(defun every (test list)
  (if (null list)
      t
      (if (test (car list))
          (every test (cdr list))
          nil)))

(defun map (fn . lists)
  (defun take (fn lists)
    (if (null lists)
        nil
        (cons (fn (car lists))
              (take fn (cdr lists)))))
  (defun cars () (take car lists))
  (defun cdrs () (take cdr lists))
  (if (some null lists)
      nil
      (cons (apply fn (cars))
            (apply map fn (cdrs)))))

(defmacro when (test body)
  (list 'if test
        body
        nil))

(defmacro unless (test body)
  (list 'when (list 'not test)
        body))

(defun append args
  (cond ((null (cdr args))
	 (car args))
	((null (car args))
	 (apply append (cdr args)))
	(t (cons (caar args)
		 (apply append
			(cdar args)
			(cdr args))))))

(defmacro let (vars . body)
  (append (list (append (list 'lambda (map car vars))
			body))
	  (map cadr vars)))
